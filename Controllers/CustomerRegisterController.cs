using BetaCycle4.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BetaCycle4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerRegisterController : ControllerBase
    {
        public Tuple<Credentials, CustomerNew> splitInfo(CustomerRegister customerRegister)
        {
            Credentials credentialToPass = new();
            CustomerNew customersNewToPass = new();

            credentialToPass.EmailAddress = customerRegister.EmailAddress;
            credentialToPass.Password = customerRegister.PasswordHash + "|" + customerRegister.PasswordSalt;
            credentialToPass.CredentialsCnnId = customerRegister.CustomerId;

            customersNewToPass.CustomerId = customerRegister.CustomerId;
            customersNewToPass.NameStyle = customerRegister.NameStyle;
            customersNewToPass.Title = customerRegister.Title;
            customersNewToPass.FirstName = customerRegister.FirstName;
            customersNewToPass.MiddleName = customerRegister.MiddleName;
            customersNewToPass.LastName = customerRegister.LastName;
            customersNewToPass.Suffix = customerRegister.Suffix;
            customersNewToPass.CompanyName = customerRegister.CompanyName;
            customersNewToPass.SalesPerson = customerRegister.SalesPerson;
            customersNewToPass.Phone = customerRegister.Phone;
            customersNewToPass.Rowguid = customerRegister.Rowguid;
            customersNewToPass.ModifiedDate = customerRegister.ModifiedDate;

            return Tuple.Create(credentialToPass, customersNewToPass);
        }      
    }
}
