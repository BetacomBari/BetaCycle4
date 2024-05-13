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

        #region COSTRUTTORE SqlConnectionString DB
        public DbUtility(string sqlConnectionString)
        {
            sqlCnn.ConnectionString = sqlConnectionString;
            try
            {
<<<<<<< HEAD
                int x = 0;
                Console.WriteLine(1 / x);
                using SqlConnection sqlConnection = sqlCnn;
                sqlConnection.Open(); //lo apro, se va tutto bene reinizializza la connessione 
                sqlCnn = new SqlConnection(sqlConnection.ConnectionString);
                IsDbStatusValid = true;
=======
                using (SqlConnection sqlConnection = sqlCnn)
                {
                    checkDbOpen(); //lo apro, se va tutto bene reinizializza la connessione 
                    sqlCnn = new SqlConnection(sqlConnection.ConnectionString);
                    IsDbStatusValid = true;
                }
>>>>>>> JWT
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

        #region CheckIsElseWhere
        internal bool CheckIsElseWhere(string email)
        {
            bool emailFlag = false;
            try
            {
                checkDbOpen();
                sqlCmd.CommandText = "SELECT EmailAddress, IsElseWhere FROM SalesLT.Customer WHERE SalesLT.Customer.EmailAddress = @email";
                sqlCmd.Parameters.AddWithValue("@email", email);
                sqlCmd.Connection = sqlCnn;

                using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                {
                    if (sqlReader.HasRows)
                    {
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
        internal int SelectIdCCustomerNew(string EmailAddress)
        {
            int id = 0;
            try
            {
                checkDbOpen();
                sqlCmd.CommandText = "SELECT CustomerId FROM CustomerNew WHERE EmailAddress = @EmailAddress";
                sqlCmd.Parameters.AddWithValue("@EmailAddress", EmailAddress);
                sqlCmd.Connection = sqlCnn;

                using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                {
                    if (sqlReader.HasRows)
                    {
                        while (sqlReader.Read())
                        {
                            id = Convert.ToInt16(sqlReader["CustomerId"]);
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

            return id;
        }
        #endregion

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

        #region DeleteCustomerNew
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
            }
            finally
            { checkDbClose(); }

            return update;
        }
        #endregion

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
<<<<<<< HEAD
    #endregion
=======
>>>>>>> JWT
}
