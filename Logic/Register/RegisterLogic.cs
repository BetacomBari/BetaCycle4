using BetaCycle4.Logic.Authentication.EncryptionWithSha256;
using BetaCycle4.Migrations;
using BetaCycle4.Models;
using Microsoft.AspNetCore.Mvc;
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


        #region PostCustomerNew
        public async Task<CustomerNew> PostCustomerNew(CustomerNew customerNew)
        {
            _context.CustomerNews.Add(customerNew);
            await _context.SaveChangesAsync();

            return customerNew;
        }
        #endregion

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
