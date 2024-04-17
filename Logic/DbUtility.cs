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


namespace SqlManager.BLogic
{
    internal class DBUtility
    {
        SqlConnection sqlCnn = new();
        SqlCommand sqlCmd = new();
        public bool IsDbStatusValid = false;


        public DBUtility(string sqlConnectionString)
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
        }


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


        internal string getPasswordFromEmail(string email)
        {
            string passwordFromDb = "";
            try
            {
                checkDbOpen();
                sqlCmd.CommandText = "SELECT password from SalesLT.Customer WHERE SalesLt.Customer.Email = @email";
                sqlCmd.Parameters.AddWithValue("@email", email);
                sqlCmd.Connection = sqlCnn;

                using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                {
                    if (sqlReader.HasRows)
                    {
                        while (sqlReader.Read())
                        {
                            passwordFromDb = sqlReader["password"].ToString();
                        }
                    }
                }
                checkDbClose();
            }
            catch (Exception)
            {
                throw;
            }

            return passwordFromDb;
        }

        internal bool CheckIsElseWhere(string email)
        {
            bool emailFlag = false;
            try
            {
                checkDbOpen();
                sqlCmd.CommandText = "SELECT email, iselsewhere FROM SalesLT.Customer WHERE SalesLT.Customer.Email = @email";
                sqlCmd.Parameters.AddWithValue("@email", email);
                sqlCmd.Connection = sqlCnn;

                using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                {
                    if (sqlReader.HasRows)
                    {
                        while (sqlReader.Read())
                        {
                            if (Convert.ToBoolean(sqlReader["iselsewhere"]) == false)
                                emailFlag = true;
                        }
                    }

                    checkDbClose();
                }
            }
            catch (Exception e)
            {
                throw;
            }


            return emailFlag;
        }
    }
}
