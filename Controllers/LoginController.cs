using BetaCycle4.Logic;
using BetaCycle4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SqlManager.BLogic;
using System.Configuration;

namespace BetaCycle4.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class LoginController : ControllerBase
    {
        [HttpPost]
        public IActionResult Login(LoginCredentials credentials)
        {

            DbUtility dbUtilityLT2019 = new("Data Source=.\\SQLEXPRESS;Initial Catalog=AdventureWorksLT2019;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            DbUtility dbUtilityCredentials = new("Data Source=.\\SQLEXPRESS;Initial Catalog=CustomerCredentials;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

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
                    if (  dbUtilityCredentials.CheckEmailDbCustomerCredentials(inputEmail)  )
                    {
                        KeyValuePair<string, string> keyValuePair = dbUtilityCredentials.GetPasswordHashAndSalt(inputEmail);

                        string passwordHash = keyValuePair.Key;
                        string passwordSalt = keyValuePair.Value;

                        bool isLogin = PasswordLogic.Encrypted(passwordHash, passwordSalt, inputPassword);

                        if (isLogin == true)
                        {
                            return Ok();
                        }
                        else
                        {
                            return BadRequest();
                        }
                    }       
                }
                else if (isElseWhere == false && emailExists == true)
                {
                    //L'UTENTE VERRÀ INDIRIZZATO NELLA PAGINA PER LE REGISTRAZIONE
                    return BadRequest();
                }
                else if (emailExists == false)
                {
                    if (dbUtilityCredentials.CheckEmailDbCustomerCredentials(inputEmail))
                    {
                        KeyValuePair<string, string> keyValuePair = dbUtilityCredentials.GetPasswordHashAndSalt(inputEmail);

                        string passwordHash = keyValuePair.Key;
                        string passwordSalt = keyValuePair.Value;

                        bool isLogged = PasswordLogic.Encrypted(passwordHash, passwordSalt, inputPassword);

                        if (isLogged == true)
                        {
                            return Ok();
                        }
                        else
                        {
                            return BadRequest();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            return BadRequest();
        }
    }
}
