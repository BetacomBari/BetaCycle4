using System.Security.Principal;

namespace WebAca5CodeFirst.Logic.Autentication.Basic
{

    public class AuthenticatedUser : IIdentity
    {
        public AuthenticatedUser(string authType, bool isAuthenticated, string name)
        {
            AuthenticationType = authType;
            IsAuthenticated = isAuthenticated;
            Name = name;
        


            // this is a comment
        }


        public string? AuthenticationType { get; set; }

        public bool IsAuthenticated { get; set; }

        public string? Name { get; set; }
    }
}
