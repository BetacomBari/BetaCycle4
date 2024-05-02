using BetaCycle4.Logic;
using BetaCycle4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SqlManager.BLogic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BetaCycle4.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class LoginController : ControllerBase
    {

        private JwtSettings _jwtSettings;

        public LoginController(JwtSettings jwtSettings)
        {
            _jwtSettings = jwtSettings;
        }

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
                            var token = GenerateJwtToken(inputEmail);


                            return Ok(new { token });
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
                            var token = GenerateJwtToken(inputEmail);


                            return Ok(new { token });
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


        #region GenerateJwtToken
        private string GenerateJwtToken(string username)
        {

            var secretKey = _jwtSettings.SecretKey;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username)
                }),

                Expires = DateTime.Now.AddMinutes(_jwtSettings.ExpirationMinutes),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(token);


            return tokenString;
        }
        #endregion
    }
}
