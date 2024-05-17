using BetaCycle4.Logic.Authentication.EncryptionWithSha256;
using BetaCycle4.Migrations;
using BetaCycle4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SqlManager.BLogic;
using System.Data.SqlTypes;

namespace BetaCycle4.Logic.Register
{
    public class RegisterLogic
    {
        private readonly AdventureWorksLt2019Context _context;

        public RegisterLogic(AdventureWorksLt2019Context context)
        {
            _context = context;
        }

        // UTILITY DB ADLT2019
        #region DeleteCustomerNew
        public bool DeleteCustomerNew(int id)
        {
            var customerNew = _context.CustomerNews.Find(id);
            if (customerNew == null)
            {
                return false;
            }

            _context.CustomerNews.Remove(customerNew);
            _context.SaveChanges();

            return true;
        }
        #endregion

        #region SetEmailNull in customerNew
        private bool CustomerNewExists(int id)
        {
            return _context.CustomerNews.Any(e => e.CustomerId == id);
        }

        public bool SetEmailNull(int id, CustomerNew customerNew)
        {
            if (id != customerNew.CustomerId)
            {
                return false;
            }

            _context.Entry(customerNew).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerNewExists(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;
        }
        #endregion

        #region PostCustomerNew
        public bool PostCustomerNew(CustomerNew customerNew)
        {
            try
            {
                _context.CustomerNews.Add(customerNew);
                _context.SaveChanges();
                return true; // Il salvataggio è andato a buon fine
            }
            catch (Exception ex)
            {
                // Gestisci eventuali eccezioni che potrebbero essere sollevate durante il salvataggio
                Console.WriteLine($"Si è verificato un errore durante il salvataggio nel database: {ex.Message}");
                return false; // Il salvataggio non è riuscito
            }
        }
        #endregion


        // UTILITY DB CREDENTIALS
        #region PostCredentials
        public bool PostCredentials(Credentials credentials)
        {

            DbUtility dbUtilityCredentials = new("Data Source=.\\SQLEXPRESS;Initial Catalog=CustomerCredentials;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            DbUtility dbUtilityLT2019 = new("Data Source=.\\SQLEXPRESS;Initial Catalog=AdventureWorksLT2019;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");


            string inputEmail = credentials.EmailAddress;

            bool emailExists = false;
            bool isElseWhere = false;
            try
            {
                emailExists = dbUtilityLT2019.CheckEmailDbAWLT2019(inputEmail);
                if (emailExists == true)
                {
                    isElseWhere = dbUtilityLT2019.CheckIsElseWhere(inputEmail);
                }
                if (isElseWhere == true && emailExists == true)
                {
                    return false;
                }
                else if (isElseWhere == false && emailExists == true)
                {
                    if (dbUtilityCredentials.CheckEmailDbCustomerCredentials(inputEmail))
                    {
                        return false;
                    }
                    else
                    {
                        if (dbUtilityCredentials.InsertCredentials(credentials) == 1)
                        {
                            return true;
                        }
                    }
                }
                else if (emailExists == false)
                {
                    if (dbUtilityCredentials.CheckEmailDbCustomerCredentials(inputEmail))
                    {
                        return false;
                    }
                    else
                    {
                        if (dbUtilityCredentials.InsertCredentials(credentials) == 1)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }
        #endregion

    }
}
