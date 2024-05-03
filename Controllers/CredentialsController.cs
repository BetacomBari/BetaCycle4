using BetaCycle4.Logic;
using BetaCycle4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SqlManager.BLogic;

namespace BetaCycle4.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CredentialsController : ControllerBase
    {
        [HttpPost]
        public bool PostCredentials(Credentials credentials)
        {

            DbUtility dbUtilityCredentials = new("Data Source=.\\SQLEXPRESS;Initial Catalog=CustomerCredentials;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            DbUtility dbUtilityLT2019 = new("Data Source=.\\SQLEXPRESS;Initial Catalog=AdventureWorksLT2019;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");


            string inputEmail = credentials.EmailAddress;
            string inputPassword = credentials.Password;

            bool emailExists = false;
            bool isElseWhere = false;
            try
            {
                emailExists = dbUtilityLT2019.CheckEmailDbAWLT2019(inputEmail);
                if (emailExists == true)
                {
                    isElseWhere = dbUtilityLT2019.CheckIsElseWhere(inputEmail);
                }

                //CONTROLLO SE L'UTENTE È PRESENTE NELLA TABELLA VECCHIA E IN QUELLA NUOVA
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
                        
                        return true;
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
                        //insert di credentials nel db credential
                        return true;
                    }
                }

            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }
    }

}

