using Microsoft.AspNetCore.Authorization;

namespace WebAca5CodeFirst.Logic.Autentication.Basic
{
    public class BasicAuthorizationAttributes: AuthorizeAttribute
    {
        public BasicAuthorizationAttributes() 
        {
            Policy = "BasicAuthentication";
        }
    }
}
