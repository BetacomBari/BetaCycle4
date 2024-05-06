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
using BetaCycle4.Logic.Authentication.EncryptionWithSha256;
using BetaCycle4.Logic;


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
                using SqlConnection sqlConnection = sqlCnn;
                sqlConnection.Open(); //lo apro, se va tutto bene reinizializza la connessione 
                sqlCnn = new SqlConnection(sqlConnection.ConnectionString);
                IsDbStatusValid = true;
            }
            catch (Exception)
            {
                throw;
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
        
        #region GetPasswordHashEndSalt From DB CustomerCredentials
        internal KeyValuePair<string,string> GetPasswordHashAndSalt(string email)
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

        #region INSERT CREDENTIALS
        internal int InsertCredentials(Credentials credentials)
        {
            int credentialsInsert = 0;
            try
            {
                checkDbOpen();

                string emailAddressEncrypt = EncryptionSHA256.sha256Encrypt(credentials.EmailAddress);

                KeyValuePair<string, string> passwordHashSalt= PasswordLogic.GetPasswordHashAndSalt(credentials.Password);

                sqlCmd.CommandText = "INSERT INTO [dbo].[Credentials] ([EmailAddressEncrypt] ,[PasswordHash] ,[PasswordSalt] ,[CredentialsCnnId]) VALUES (@EmailAddressEncrypt, @PasswordHash, @PasswordSalt, )";
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
