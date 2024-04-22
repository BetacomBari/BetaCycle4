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
            catch (Exception e)
            {
                ErrorManager.RegisterError(e.Message, Convert.ToInt16(e.HResult), System.Reflection.MethodBase.GetCurrentMethod().Name);
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
                sqlCmd.CommandText = "SELECT EmailAddress FROM [dbo].[Credentials] WHERE [dbo].[Credentials].EmailAddress = @email";
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
                sqlCmd.CommandText = "SELECT PasswordHash, PasswordSalt from [dbo].[Credentials] WHERE [dbo].[Credentials].EmailAddress = @email";
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

        #region ErrorWriter in LOGERROR

        /// <summary>
        /// Metodo unico richiamato dalla classe statica per la scrittura degli errori nella tabella apposita.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="code"></param>
        /// <param name="location"></param>
        public void ErrorWriter(string message, short code, string location)
        {
            checkDbOpen();

            sqlCmd.CommandText = "INSERT INTO LogError (ErrorMessage, ErrorCode, ErrorDate, ErrorLocation) VALUES (@message, @code, @date, @location)";
            sqlCmd.Parameters.AddWithValue("@message", message);
            sqlCmd.Parameters.AddWithValue("@code", code);
            sqlCmd.Parameters.AddWithValue("@date", DateTime.UtcNow);
            sqlCmd.Parameters.AddWithValue("@location", location);

            sqlCmd.ExecuteNonQuery();

            checkDbClose();
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
