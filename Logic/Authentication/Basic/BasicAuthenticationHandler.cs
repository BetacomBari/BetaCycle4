using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using SqlManager.BLogic;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using WebAca5CodeFirst.Logic.Autentication;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using Humanizer.Configuration;
using System.Security.Cryptography.Xml;
using BetaCycle4.Logic.Authentication.EncryptionWithSha256;



namespace WebAca5CodeFirst.Logic.Autentication.Basic
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        //public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, 
        //    ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
        //{
        //}
        private readonly IConfiguration _configuration;
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IConfiguration configuration) :
            base(options, logger, encoder, clock)
        {
            _configuration = configuration;

        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Response.Headers.Add("WWW-Authenticate", "Basic");

            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Autorizzazione mancante"));
            }

            var authorizationHeader = Request.Headers["Authorization"].ToString();
            var authorHeaderRegex = new Regex("Basic (.*)");

            if (!authorHeaderRegex.IsMatch(authorizationHeader))
            {
                return Task.FromResult(AuthenticateResult.Fail("Autorizzazione mancante"));
            }

            var auth64 = Encoding.UTF8.GetString(
                Convert.FromBase64String(authorHeaderRegex.
                Replace(authorizationHeader, "$1")));

            var authArraySplit = auth64.Split(Convert.ToChar(':'), 2);
            var authUser = authArraySplit[0];
            var authPassword = authArraySplit.Length > 1 ? authArraySplit[1] : throw new Exception("Password non presente");

            if (string.IsNullOrEmpty(authUser) || string.IsNullOrEmpty(authPassword))
            {
                return Task.FromResult(AuthenticateResult.Fail("Username e/o Password non validi"));
            }
            else
            {
                string passwordFromInput = "";
                var connecion_string = _configuration.GetConnectionString("ConnectionString");
                DBUtility dBUtility = new(connecion_string);
                if (dBUtility.IsDbStatusValid)
                {
                    string passwordHashFromDb = dBUtility.getPasswordFromEmail(authUser);
                    authPassword = EncryptionSHA256.sha256Encrypt(passwordFromInput);
                    if (passwordHashFromDb == authPassword)
                    {
                        // authPassword gi� con hash? se no va bene come sta fatto
                        Console.WriteLine("Login effettuato con successo");
                    }
                }
            }

              



            

            var authenticatedUser = new AuthenticatedUser("BasicAuthentication", true, authArraySplit[0].ToString());
            var claimsMain = new ClaimsPrincipal(new ClaimsIdentity(authenticatedUser));

            return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsMain, Scheme.Name)));

        }
    }
}
