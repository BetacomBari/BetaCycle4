using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using BetaCycle4.Models;
using System.ComponentModel;

using BetaCycle4.Logger;

using BetaCycle4.Logic;

using BetaCycle4.Logic.Authentication.EncryptionWithSha256;

using System.Security.Cryptography;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.Net;
using System.Net.Mail;


namespace SqlManager.BLogic
{
    internal class DbUtility
    {
        SqlConnection sqlCnn = new();
        SqlCommand sqlCmd = new();
        public bool IsDbStatusValid = false;

        //COSTRUTTORE
        #region COSTRUTTORE SqlConnectionString DB
        public DbUtility(string sqlConnectionString)
        {
            sqlCnn.ConnectionString = sqlConnectionString;
            try
            {

                using (SqlConnection sqlConnection = sqlCnn)
                {
                    checkDbOpen(); //lo apro, se va tutto bene reinizializza la connessione 
                    sqlCnn = new SqlConnection(sqlConnection.ConnectionString);
                    IsDbStatusValid = true;
                }

            }
            catch (Exception e)
            {
                DbTracer error = new DbTracer();
                error.InsertError(e.Message, e.HResult, e.StackTrace);
            }
            finally
            {
                checkDbClose();
            }
        }
        #endregion


        //UTILITY DB  CREDENTIALS
        #region update CredentialId 
        internal int UpdateCredentialId(CustomerNew customerNew, int customerId)
        {
            int credentialsUpdate = 0;
            try
            {
                checkDbOpen();
                sqlCmd.Connection = sqlCnn;
                sqlCmd.CommandText = "UPDATE [dbo].[Credentials] SET [CredentialsCnnId] = @CredentialsCnnId WHERE EmailAddressEncrypt = @EmailAddressEncrypt";
                sqlCmd.Parameters.AddWithValue("@EmailAddressEncrypt", customerNew.EmailAddress);
                sqlCmd.Parameters.AddWithValue("@CredentialsCnnId", customerId);


                credentialsUpdate = sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            { checkDbClose(); }

            return credentialsUpdate;
        }
        #endregion

        #region CheckEmailDbCustomerCredentials
        internal bool CheckEmailDbCustomerCredentials(string email)
        {
            bool emailExists = false;
            try
            {
                checkDbOpen();

                //Criptazione della mail dato che nel db è criptata
                email = EncryptionSHA256.sha256Encrypt(email);


                sqlCmd.CommandText = "SELECT EmailAddressEncrypt FROM [dbo].[Credentials] WHERE [dbo].[Credentials].EmailAddressEncrypt = @email";
                sqlCmd.Parameters.AddWithValue("@email", email);
                sqlCmd.Connection = sqlCnn;

                using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                {
                    if (sqlReader.HasRows)
                    {
                        emailExists = true;
                    }
                    else
                    {
                        emailExists = false;
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                checkDbClose();
            }

            return emailExists;
        }
        #endregion

        #region credentialsFromEmail
        internal CredentialDB credentialsFromEmail(string email)
        {
            CredentialDB credentials = new CredentialDB();
            //var tokenBytes = RandomNumberGenerator.GetBytes(64);
            //var emailToken = Convert.ToBase64String(tokenBytes);
            //emailToken = emailToken.Replace(" ", "+");

            try
            {
                checkDbOpen();

                //Criptazione della mail dato che nel db è criptata
                string emailEncrypt = EncryptionSHA256.sha256Encrypt(email);

                sqlCmd.CommandText = "SELECT * FROM [dbo].[Credentials] WHERE [dbo].[Credentials].EmailAddressEncrypt = @email";
                sqlCmd.Parameters.AddWithValue("@email", emailEncrypt);
                sqlCmd.Connection = sqlCnn;

                using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                {
                    if (sqlReader.HasRows)
                    {
                        while (sqlReader.Read())
                        {
                            credentials.EmailAddressEncrypt = sqlReader["EmailAddressEncrypt"].ToString();
                            credentials.PasswordHash = sqlReader["PasswordHash"].ToString();
                            credentials.PasswordSalt = sqlReader["PasswordSalt"].ToString();
                            credentials.ResetPasswordToken = sqlReader["ResetPasswordToken"].ToString();
                            //credentials.ResetPasswordToken = emailToken;
                            credentials.ResetPasswordExpiry = (sqlReader["ResetPasswordExpiry"]) as DateTime?;
                            if (credentials.ResetPasswordExpiry == null)
                            {
                                // Set default value here
                                credentials.ResetPasswordExpiry = DateTime.Now; // Example default (reset in 30 days)
                            }
                            credentials.CredentialsCnnId = Convert.ToInt16(sqlReader["CredentialsCnnId"]);
                        }

                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                checkDbClose();
            }

            return credentials;
        }
        #endregion

        #region writeInfoOnDb
        internal void writeInfoOnDb(CredentialDB credentialDBToInsert)
        {
            CredentialDB credentialDb = new CredentialDB();
            try
            {
                checkDbOpen();

                //string emailAddressEncrypt = EncryptionSHA256.sha256Encrypt(credentials.EmailAddress);

                //KeyValuePair<string, string> passwordHashSalt = PasswordLogic.GetPasswordHashAndSalt(credentials.Password);

                sqlCmd.CommandText = "UPDATE [dbo].[Credentials] SET PasswordHash=@PasswordHash ,PasswordSalt=@PasswordSalt ,CredentialsCnnId=@CredentialsCnnId, ResetPasswordToken=@ResetPasswordToken, ResetPasswordExpiry=@ResetPasswordExpiry WHERE EmailAddressEncrypt=@EmailAddressEncrypt";
                sqlCmd.Parameters.AddWithValue("@EmailAddressEncrypt", credentialDBToInsert.EmailAddressEncrypt);
                sqlCmd.Parameters.AddWithValue("@PasswordHash", credentialDBToInsert.PasswordHash);
                sqlCmd.Parameters.AddWithValue("@PasswordSalt", credentialDBToInsert.PasswordSalt);
                sqlCmd.Parameters.AddWithValue("@CredentialsCnnId", credentialDBToInsert.CredentialsCnnId);
                sqlCmd.Parameters.AddWithValue("@ResetPasswordToken", credentialDBToInsert.ResetPasswordToken);
                sqlCmd.Parameters.AddWithValue("@ResetPasswordExpiry", credentialDBToInsert.ResetPasswordExpiry);


                sqlCmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            { checkDbClose(); }


        }
        #endregion

        #region writeNewPasswordOnDb
        internal void writeNewPasswordOnDb(BetaCycle4.Models.ResetPassword resetPassword)
        {
            string passwordHash;
            string passwordSalt;
            KeyValuePair<string, string> passwordHashSalt = PasswordLogic.GetPasswordHashAndSalt(resetPassword.NewPassword);
            try
            {
                checkDbOpen();

                string emailAddressEncrypt = EncryptionSHA256.sha256Encrypt(resetPassword.Email);

                //KeyValuePair<string, string> passwordHashSalt = PasswordLogic.GetPasswordHashAndSalt(credentials.Password);

                sqlCmd.CommandText = "UPDATE [dbo].[Credentials] SET PasswordHash=@PasswordHash ,PasswordSalt=@PasswordSalt WHERE EmailAddressEncrypt=@EmailAddressEncrypt";
                sqlCmd.Parameters.AddWithValue("@EmailAddressEncrypt", emailAddressEncrypt);
                sqlCmd.Parameters.AddWithValue("@PasswordHash", passwordHashSalt.Key);
                sqlCmd.Parameters.AddWithValue("@PasswordSalt", passwordHashSalt.Value);



                sqlCmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            { checkDbClose(); }


        }
        #endregion

        #region GetPasswordHashEndSalt From DB CustomerCredentials
        internal KeyValuePair<string, string> GetPasswordHashAndSalt(string email)
        {
            string? pwrHash = string.Empty;
            string? pwrSalt = string.Empty;
            try
            {
                checkDbOpen();

                //Criptazione della mail dato che nel db è criptata
                email = EncryptionSHA256.sha256Encrypt(email);

                sqlCmd.CommandText = "SELECT PasswordHash, PasswordSalt from [dbo].[Credentials] WHERE [dbo].[Credentials].EmailAddressEncrypt = @email";
                sqlCmd.Parameters.AddWithValue("@email", email);
                sqlCmd.Connection = sqlCnn;

                using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                {
                    if (sqlReader.HasRows)
                    {
                        while (sqlReader.Read())
                        {
                            pwrHash = sqlReader["PasswordHash"].ToString();
                            pwrSalt = sqlReader["PasswordSalt"].ToString();
                        }
                    }
                }
                checkDbClose();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                checkDbClose();
            }

            return new KeyValuePair<string, string>(pwrHash, pwrSalt);
        }
        #endregion

        #region Insert Ccredentials
        internal int InsertCredentials(Credentials credentials)
        {
            int credentialsInsert = 0;
            try
            {
                checkDbOpen();

                string emailAddressEncrypt = EncryptionSHA256.sha256Encrypt(credentials.EmailAddress);

                KeyValuePair<string, string> passwordHashSalt = PasswordLogic.GetPasswordHashAndSalt(credentials.Password);

                sqlCmd.CommandText = "INSERT INTO [dbo].[Credentials] ([EmailAddressEncrypt] ,[PasswordHash] ,[PasswordSalt] ,[CredentialsCnnId]) VALUES (@EmailAddressEncrypt, @PasswordHash, @PasswordSalt, @CredentialsCnnId)";
                sqlCmd.Parameters.AddWithValue("@EmailAddressEncrypt", emailAddressEncrypt);
                sqlCmd.Parameters.AddWithValue("@PasswordHash", passwordHashSalt.Key);
                sqlCmd.Parameters.AddWithValue("@PasswordSalt", passwordHashSalt.Value);
                sqlCmd.Parameters.AddWithValue("@CredentialsCnnId", credentials.CredentialsCnnId);


                credentialsInsert = sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            { checkDbClose(); }

            return credentialsInsert;
        }
        #endregion

        #region Update token Ccredentials
        internal int UpdateTokenCredentials(string EmailAddressEncrypt, string ResetPasswordToken, DateTime ResetPasswordExpiry)
        {
            int update = 0;

            try
            {
                checkDbOpen();
                sqlCmd.Connection = sqlCnn;
                sqlCmd.CommandText = "UPDATE [dbo].[Credentials] SET [ResetPasswordToken] = @ResetPasswordToken ,[ResetPasswordExpiry] = @ResetPasswordExpiry WHERE [EmailAddressEncrypt] = @EmailAddressEncrypt";
                sqlCmd.Parameters.AddWithValue("@EmailAddressEncrypt", EmailAddressEncrypt);
                sqlCmd.Parameters.AddWithValue("@ResetPasswordToken", ResetPasswordToken);
                sqlCmd.Parameters.AddWithValue("@ResetPasswordExpiry", ResetPasswordExpiry);


                update = sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRORE: {ex.Message}");
                return update = 0;
            }
            finally
            { checkDbClose(); }

            return update;
        }
        #endregion

        #region DeleteCredentials
        public int DeleteCredentials(int CredentialsCnnId)
        {
            int delete = 0;

            try
            {
                checkDbOpen();
                sqlCmd.Connection = sqlCnn;
                sqlCmd.CommandText = "DELETE FROM [dbo].[Credentials] WHERE CredentialsCnnId = @CredentialsCnnId";
                sqlCmd.Parameters.AddWithValue("@CredentialsCnnId", CredentialsCnnId);

                delete = sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRORE: {ex.Message}");
            }
            finally
            { checkDbClose(); }

            return delete;
        }
        #endregion


        //DB UTILITY ADLT2019
        #region CheckIsElseWhere
        internal bool CheckIsElseWhere(string email)
        {
            bool emailFlag = false;
            try
            {
                checkDbOpen();
                sqlCmd.CommandText = "SELECT CustomerId, IsElseWhere FROM SalesLT.Customer WHERE SalesLT.Customer.EmailAddress = @email";
                sqlCmd.Parameters.AddWithValue("@email", email);
                sqlCmd.Connection = sqlCnn;

=======
        #region Get All Product Categories

        internal List<ProductCategory> getAllCategories()
        {
            List<ProductCategory> allCategories = new();
            try
            {
                checkDbOpen();

                sqlCmd.CommandText = "SELECT * FROM [AdventureWorksLT2019].[SalesLT].[ProductCategory]";

                sqlCmd.Connection = sqlCnn;
>>>>>>> Product
                using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                {
                    if (sqlReader.HasRows)
                    {
<<<<<<< HEAD
                        while (sqlReader.Read())
                        {
                            if (Convert.ToBoolean(sqlReader["iselsewhere"]) == true)
                                emailFlag = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                checkDbClose();
            }

            return emailFlag;
        }
        #endregion

        #region CheckEmailDbAWLT2019
        internal bool CheckEmailDbAWLT2019(string email)
        {
            bool emailExists = false;
            try
            {
                checkDbOpen();
                sqlCmd.CommandText = "SELECT EmailAddress FROM SalesLT.Customer WHERE SalesLT.Customer.EmailAddress = @email";
                sqlCmd.Parameters.AddWithValue("@email", email);
                sqlCmd.Connection = sqlCnn;

                using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                {
                    if (sqlReader.HasRows)
                    {
                        emailExists = true;
                    }
                    else
                    {
                        emailExists = false;
                    }
                }

            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                checkDbClose();
            }

            return emailExists;
        }
        #endregion

        #region SelectID By curtomerNew
        internal int SelectIdCustomerNew(string EmailAddress)
        {
            int id = 0;
            try
            {
                checkDbOpen();
                sqlCmd.CommandText = "SELECT CustomerId FROM CustomerNew WHERE EmailAddress = @EmailAddress";
                sqlCmd.Parameters.AddWithValue("@EmailAddress", EmailAddress);
=======
                        ProductCategory category = new ProductCategory();
                        while (sqlReader.Read())
                        {
                            category.ProductCategoryId = Convert.ToInt16(sqlReader["ProductCategoryId"]);
                            category.ParentProductCategoryId = Convert.ToInt16(sqlReader["ParentProductCategoryId"]);
                            category.Name = sqlReader["Name"].ToString();
                            category.Rowguid = (Guid)sqlReader["Rowguid"];
                            category.ModifiedDate = Convert.ToDateTime(sqlReader["ModifiedDate"]);

                            allCategories.Add(category);
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch
            {

            }



            return allCategories;
        }

        #endregion

        #region Get All Product From Category Id

        internal List<Product> getAllProductFromCategoryId(string categoryId)
        {
            List<Product> allProductsByCategoryId = new();
            try
            {
                checkDbOpen();

                sqlCmd.CommandText = "SELECT * FROM [AdventureWorksLT2019].[SalesLT].[Product] WHERE [ProductCategoryID] = @categoryId";
                sqlCmd.Parameters.AddWithValue("@categoryId", categoryId);
>>>>>>> Product
                sqlCmd.Connection = sqlCnn;

                using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                {
                    if (sqlReader.HasRows)
                    {
<<<<<<< HEAD
                        while (sqlReader.Read())
                        {
                            id = Convert.ToInt16(sqlReader["CustomerId"]);
                        }
=======
                        Product product = new Product();
                        while (sqlReader.Read())
                        {
                            product.ProductId = Convert.ToInt16(sqlReader["ProductId"]);
                            product.Name = sqlReader["Name"].ToString();
                            product.ProductNumber = sqlReader["ProductNumber"].ToString();
                            product.Color = sqlReader["Color"].ToString();
                            product.StandardCost = Convert.ToInt16(sqlReader["StandardCost"]);
                            product.ListPrice = Convert.ToInt16(sqlReader["ListPrice"]);
                            product.Size = sqlReader["Size"].ToString();
                            product.Weight = Convert.ToDecimal(sqlReader["Weight"]);
                            product.ProductCategoryId = Convert.ToInt16(sqlReader["ProductCategoryId"]);
                            product.ProductModelId = Convert.ToInt16(sqlReader["ProductModelId"]);
                            product.SellStartDate = Convert.ToDateTime(sqlReader["SellStartDate"]);
                            product.SellEndDate = Convert.ToDateTime(sqlReader["SellEndDate"]);
                            product.DiscontinuedDate = Convert.ToDateTime(sqlReader["DiscontinuedDate"]);
                            product.ThumbNailPhoto = [Convert.ToByte(sqlReader["ThumbNailPhoto"])];
                            product.ThumbnailPhotoFileName = sqlReader["ThumbnailPhotoFileName"].ToString();
                            product.Rowguid = (Guid)sqlReader["Rowguid"];
                            product.ModifiedDate = Convert.ToDateTime(sqlReader["ModifiedDate"]);
                            product.LargeImage = [Convert.ToByte(sqlReader["LargeImage"])];

                            allProductsByCategoryId.Add(product);
                        }

                    }
                    else
                    {
                        return null;
>>>>>>> Product
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                checkDbClose();
            }
<<<<<<< HEAD

            return id;
        }
        #endregion

        #region SelectIDaddress
        internal int SelectAddressId(int CustomerId)
        {
            int id = 0;
            try
            {
                checkDbOpen();
                sqlCmd.CommandText = "SELECT AddressId FROM AddressNew WHERE CustomerId = @CustomerId";
                sqlCmd.Parameters.AddWithValue("@CustomerId", CustomerId);
=======
            return allProductsByCategoryId;

        }

        #endregion

        #region Get All Products From Category Name

        internal List<Product> getAllProductFromCategoryName(string categoryName)
        {
            List<Product> allProductsByCategoryName = new();
            try
            {
                checkDbOpen();

                sqlCmd.CommandText = "SELECT * FROM [AdventureWorksLT2019].[SalesLT].[Product] WHERE [ProductCategoryID] LIKE @categoryName";
                sqlCmd.Parameters.AddWithValue("@categoryName", categoryName);
>>>>>>> Product
                sqlCmd.Connection = sqlCnn;

                using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                {
                    if (sqlReader.HasRows)
                    {
<<<<<<< HEAD
                        while (sqlReader.Read())
                        {
                            id = Convert.ToInt16(sqlReader["AddressId"]);
                        }
=======
                        Product product = new Product();
                        while (sqlReader.Read())
                        {

                            product.ProductId = Convert.ToInt16(sqlReader["ProductId"]);
                            product.Name = sqlReader["Name"].ToString();
                            product.ProductNumber = sqlReader["ProductNumber"].ToString();
                            product.Color = sqlReader["Color"].ToString();
                            product.StandardCost = Convert.ToInt16(sqlReader["StandardCost"]);
                            product.ListPrice = Convert.ToInt16(sqlReader["ListPrice"]);
                            product.Size = sqlReader["Size"].ToString();
                            product.Weight = Convert.ToDecimal(sqlReader["Weight"]);
                            product.ProductCategoryId = Convert.ToInt16(sqlReader["ProductCategoryId"]);
                            product.ProductModelId = Convert.ToInt16(sqlReader["ProductModelId"]);
                            product.SellStartDate = Convert.ToDateTime(sqlReader["SellStartDate"]);
                            product.SellEndDate = Convert.ToDateTime(sqlReader["SellEndDate"]);
                            product.DiscontinuedDate = Convert.ToDateTime(sqlReader["DiscontinuedDate"]);
                            product.ThumbNailPhoto = [Convert.ToByte(sqlReader["ThumbNailPhoto"])];
                            product.ThumbnailPhotoFileName = sqlReader["ThumbnailPhotoFileName"].ToString();
                            product.Rowguid = (Guid)sqlReader["Rowguid"];
                            product.ModifiedDate = Convert.ToDateTime(sqlReader["ModifiedDate"]);
                            product.LargeImage = [Convert.ToByte(sqlReader["LargeImage"])];

                            allProductsByCategoryName.Add(product);
                        }

                    }
                    else
                    {
                        return null;
>>>>>>> Product
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                checkDbClose();
            }
<<<<<<< HEAD

            return id;
        }
        #endregion

        #region PostAddress
        internal int PostAddressNew(Address address)
        {
            int addressInsert = 0;
=======
            return allProductsByCategoryName;

        }


        #endregion

        #region Get Product From Name

        internal List<Product> getProductFromName(string name)
        {
            List<Product> allProductFromName = new();
>>>>>>> Product
            try
            {
                checkDbOpen();

<<<<<<< HEAD
                sqlCmd.CommandText = "INSERT INTO [dbo].[AddressNew] ([CustomerID],[AddressLine1],[AddressLine2],[City],[StateProvince],[CountryRegion],[PostalCode],[ModifiedDate]) VALUES (@CustomerID,@AddressLine1,@AddressLine2,@City,@StateProvince,@CountryRegion,@PostalCode,@ModifiedDate)";
                sqlCmd.Parameters.AddWithValue("@CustomerID", address.CustomerId);
                sqlCmd.Parameters.AddWithValue("@AddressLine1", address.AddressLine1);
                sqlCmd.Parameters.AddWithValue("@AddressLine2", address.AddressLine2);
                sqlCmd.Parameters.AddWithValue("@City", address.City);
                sqlCmd.Parameters.AddWithValue("@StateProvince", address.StateProvince);
                sqlCmd.Parameters.AddWithValue("@CountryRegion", address.CountryRegion);
                sqlCmd.Parameters.AddWithValue("@PostalCode", address.PostalCode);
                sqlCmd.Parameters.AddWithValue("@ModifiedDate", address.ModifiedDate);


                addressInsert = sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return addressInsert = 0;
                throw;
            }
            finally
            { checkDbClose(); }

            return addressInsert;
        }
        #endregion

        #region PostCustomerAddressNew
        internal int PostCustomerAddressNew(CustomerAddress customerAddress)
        {
            int CustomerAddressInsert = 0;
            try
            {
                checkDbOpen();

                sqlCmd.CommandText = "INSERT INTO [dbo].[CustomerAddressNew] ([CustomerID],[AddressID],[AddressType],[ModifiedDate]) VALUES (@CustomerID,@AddressID,@AddressType,@ModifiedDate)";
                sqlCmd.Parameters.AddWithValue("@CustomerID", customerAddress.CustomerId);
                sqlCmd.Parameters.AddWithValue("@AddressID", customerAddress.AddressId);
                sqlCmd.Parameters.AddWithValue("@AddressType", customerAddress.AddressType);
                sqlCmd.Parameters.AddWithValue("@ModifiedDate", customerAddress.ModifiedDate);



                CustomerAddressInsert = sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
=======
                sqlCmd.CommandText = "SELECT * FROM [AdventureWorksLT2019].[SalesLT].[Product] WHERE [Name] LIKE @Name";
                sqlCmd.Parameters.AddWithValue("@Name", name);
                sqlCmd.Connection = sqlCnn;

                using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                {
                    if (sqlReader.HasRows)
                    {
                        Product product = new Product();
                        while (sqlReader.Read())
                        {

                            product.ProductId = Convert.ToInt16(sqlReader["ProductId"]);
                            product.Name = sqlReader["Name"].ToString();
                            product.ProductNumber = sqlReader["ProductNumber"].ToString();
                            product.Color = sqlReader["Color"].ToString();
                            product.StandardCost = Convert.ToInt16(sqlReader["StandardCost"]);
                            product.ListPrice = Convert.ToInt16(sqlReader["ListPrice"]);
                            product.Size = sqlReader["Size"].ToString();
                            product.Weight = Convert.ToDecimal(sqlReader["Weight"]);
                            product.ProductCategoryId = Convert.ToInt16(sqlReader["ProductCategoryId"]);
                            product.ProductModelId = Convert.ToInt16(sqlReader["ProductModelId"]);
                            product.SellStartDate = Convert.ToDateTime(sqlReader["SellStartDate"]);
                            product.SellEndDate = Convert.ToDateTime(sqlReader["SellEndDate"]);
                            product.DiscontinuedDate = Convert.ToDateTime(sqlReader["DiscontinuedDate"]);
                            product.ThumbNailPhoto = [Convert.ToByte(sqlReader["ThumbNailPhoto"])];
                            product.ThumbnailPhotoFileName = sqlReader["ThumbnailPhotoFileName"].ToString();
                            product.Rowguid = (Guid)sqlReader["Rowguid"];
                            product.ModifiedDate = Convert.ToDateTime(sqlReader["ModifiedDate"]);
                            product.LargeImage = [Convert.ToByte(sqlReader["LargeImage"])];

                            allProductFromName.Add(product);
                        }

                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception e)
>>>>>>> Product
            {
                throw;
            }
            finally
<<<<<<< HEAD
            { checkDbClose(); }

            return CustomerAddressInsert;
        }
        #endregion

        #region DeleteCredentials
        public int DeleteAddressNew(int addressId)
        {
            int delete = 0;

            try
            {
                checkDbOpen();
                sqlCmd.Connection = sqlCnn;
                sqlCmd.CommandText = "DELETE FROM [dbo].[AddressNew] WHERE AddressID = @AddressID";
                sqlCmd.Parameters.AddWithValue("@AddressID", addressId);

                delete = sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRORE: {ex.Message}");
                return delete = 0;
            }
            finally
            { checkDbClose(); }

            return delete;
        }
        #endregion

        #region SetIsElseWhereTrue
        internal int SetIsElseWhereTrue(string emailAddress)
        {
            int update = 0;

            try
            {
                checkDbOpen();
                sqlCmd.Connection = sqlCnn;
                sqlCmd.CommandText = "UPDATE [SalesLT].[Customer] SET [IsElseWhere] = 1 WHERE EmailAddress = @EmailAddress";
                sqlCmd.Parameters.AddWithValue("@EmailAddress", emailAddress);

                update = sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRORE: {ex.Message}");
                return update = 0;
            }
            finally
            { checkDbClose(); }

            return update;
        }
        #endregion

        //CHECK DB

        #region CHECK OPEN/CLOSE DB
        void checkDbOpen()
        {
            if (sqlCnn.State == System.Data.ConnectionState.Closed)
            {
                sqlCnn.Open();
            }
        }

        void checkDbClose()
        {
            if (sqlCnn.State == System.Data.ConnectionState.Closed)
            {
                sqlCnn.Close();
            }
            sqlCmd.Parameters.Clear();
        }
        #endregion
    }
}
