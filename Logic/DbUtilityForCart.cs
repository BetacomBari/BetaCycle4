using BetaCycle4.Logger;
using BetaCycle4.Logic.Authentication.EncryptionWithSha256;
using BetaCycle4.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Net.Mail;

namespace BetaCycle4.Logic
{
    public class DbUtilityForCart
    {
        private DbTracer _dbTracer = new DbTracer();
        SqlConnection sqlCnn = new();
        SqlCommand sqlCmd = new();
        public bool IsDbStatusValid = false;

        //COSTRUTTORE
        #region COSTRUTTORE SqlConnectionString DB
        public DbUtilityForCart(string sqlConnectionString)
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
            catch (Exception ex)
            {
                _dbTracer.InsertError(ex.Message, ex.HResult, ex.StackTrace);
            }
            finally
            {
                checkDbClose();
            }
        }
        #endregion

        //CRUD FOR SHOPPINGCART
        #region GetCartByCustomer
        internal List<Product> GetCartByCustomerId(CustomerNew customerNew)
        {
            List<Product> productsInCart = new List<Product>();

            try
            {
                checkDbOpen();
                sqlCmd.CommandText = "SELECT ProductId FROM ShoppingCart WHERE CustomerId = @customerId AND IsCompleted = false";
                sqlCmd.Parameters.AddWithValue("@customerId", customerNew.CustomerId);
                sqlCmd.Connection = sqlCnn;

                using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                {
                    if (sqlReader.HasRows)
                    {
                        while (sqlReader.Read())
                        {
                            Product product = new Product();
                            product.ProductId = Convert.ToInt16(sqlReader["ProductId"]);
                            product = GetProductInfo(product.ProductId);
                            productsInCart.Add(product);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _dbTracer.InsertError(ex.Message, ex.HResult, ex.StackTrace);
            }
            finally
            {
                checkDbClose();
            }

            return productsInCart;
        }
        #endregion

        #region AddProductToCart
        internal void AddProudctToCart(CustomerNew customer, Product product)
        {
            try
            {
                checkDbOpen();

                sqlCmd.CommandText = "UPDATE [dbo].[ShoppingCart] SET CustomerId = @customerId ,ProductId=@productId";
                sqlCmd.Parameters.AddWithValue("@customerId", customer.CustomerId);
                sqlCmd.Parameters.AddWithValue("@productId", product.ProductId);
               
                sqlCmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                _dbTracer.InsertError(ex.Message, ex.HResult, ex.StackTrace);
            }
            finally
            {
                checkDbClose();
            }
        }
        #endregion

        #region BuyProductInYourCart
        internal void BuyProductInYourCart(CustomerNew customer, Product product)
        {
            try
            {
                checkDbOpen();

                sqlCmd.CommandText = "UPDATE [dbo].[ShoppingCart] SET IsCompleted = true WHERE CustomerId = @customerId AND ProductId = @productId";
                sqlCmd.Parameters.AddWithValue("@customerId", customer.CustomerId);
                sqlCmd.Parameters.AddWithValue("@productId", product.ProductId);

                sqlCmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                _dbTracer.InsertError(ex.Message, ex.HResult, ex.StackTrace);
            }
            finally
            {
                checkDbClose();
            }
        }
        #endregion

        #region DeleteProdutInYourCart
        internal void DeleteProdutInYourCart(CustomerNew customer, Product product)
        {
            try
            {
                checkDbOpen();

                sqlCmd.CommandText = "DELETE [dbo].[ShoppingCart] WHERE CustomerId = @customerId AND ProductId = @productId";
                sqlCmd.Parameters.AddWithValue("@customerId", customer.CustomerId);
                sqlCmd.Parameters.AddWithValue("@productId", product.ProductId);

                sqlCmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                _dbTracer.InsertError(ex.Message, ex.HResult, ex.StackTrace);
            }
            finally
            {
                checkDbClose();
            }
        }
        #endregion

        #region GetProductInfo
        internal Product GetProductInfo(int productId)
        {
            Product product = new Product();
            try
            {
                checkDbOpen();
                sqlCmd.CommandText = "SELECT * FROM Product WHERE ProductId = @productId";
                sqlCmd.Parameters.AddWithValue("@productId", productId);
                sqlCmd.Connection = sqlCnn;

                using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                {
                    if (sqlReader.HasRows)
                    {
                        while (sqlReader.Read())
                        {
                            //product.ProductId = Convert.ToInt16(sqlReader["ProductId"]);
                            product.Name = sqlReader["Name"].ToString();
                            product.Color = sqlReader["Color"].ToString();
                            product.Size = sqlReader["Size"].ToString();
                            product.ListPrice = Convert.ToDecimal(sqlReader["ListPrice"]);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _dbTracer.InsertError(ex.Message, ex.HResult, ex.StackTrace);
            }
            finally
            {
                checkDbClose();
            }

            return product;
        }

        #endregion

        internal void CreateOrder(List<int> productsToSell)
        {
            List<Product> products = new();

            //Devo ricercare i prodotti da aggiungere al carrello prima
            foreach(int productId in productsToSell)
            {
                Product singleProduct = new Product();
                singleProduct = GetProductInfo(productId);
                if (singleProduct != null) products.Add(singleProduct);
                else break;
            }
        }

        #region SelectID By curtomerNew
        internal int SelectIdCustomerNew(string EmailAddress)
        {
            int id = -1;
            try
            {
                checkDbOpen();
                string emailAddressEncrypt = EncryptionSHA256.sha256Encrypt(EmailAddress);
                sqlCmd.CommandText = "SELECT Id FROM [dbo].[Credentials] WHERE EmailAddressEncrypt = @EmailAddress";
                sqlCmd.Parameters.AddWithValue("@EmailAddress", emailAddressEncrypt);
                sqlCmd.Connection = sqlCnn;

                using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                {
                    if (sqlReader.HasRows)
                    {
                        while (sqlReader.Read())
                        {
                            id = Convert.ToInt32(sqlReader["Id"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _dbTracer.InsertError(ex.Message, ex.HResult, ex.StackTrace);
            }
            finally
            {
                checkDbClose();
            }

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
                sqlCmd.Connection = sqlCnn;

                using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                {
                    if (sqlReader.HasRows)
                    {
                        while (sqlReader.Read())
                        {
                            id = Convert.ToInt16(sqlReader["AddressId"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _dbTracer.InsertError(ex.Message, ex.HResult, ex.StackTrace);
            }
            finally
            {
                checkDbClose();
            }

            return id;
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
