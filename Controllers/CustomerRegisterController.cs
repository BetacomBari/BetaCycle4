using BetaCycle4.Logic;
using BetaCycle4.Logic.Authentication.EncryptionWithSha256;
using BetaCycle4.Logic.Register;
using BetaCycle4.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SqlManager.BLogic;

namespace BetaCycle4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerRegisterController : ControllerBase
    {
        DbUtility dbUtilityLT2019 = new("Data Source=.\\SQLEXPRESS;Initial Catalog=AdventureWorksLT2019;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        DbUtility dbUtilityCredentials = new("Data Source=.\\SQLEXPRESS;Initial Catalog=CustomerCredentials;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

        private readonly AdventureWorksLt2019Context _context;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;

        public CustomerRegisterController(AdventureWorksLt2019Context context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Register(CustomerRegister customerRegister)
        {
            CustomersNewController customersNewController = new CustomersNewController(_context, _config, _emailService);

            Credentials credentialToPass = new();
            CustomerNew customersNewToPass = new();

            credentialToPass.EmailAddress = customerRegister.EmailAddress;
            credentialToPass.Password = customerRegister.Password;
            //credentialToPass.CredentialsCnnId = dbUtilityLT2019.SelectIdCCustomerNew(customerRegister.EmailAddress);

            customersNewToPass.NameStyle = customerRegister.NameStyle;
            customersNewToPass.Title = customerRegister.Title;
            customersNewToPass.FirstName = customerRegister.FirstName;
            customersNewToPass.MiddleName = customerRegister.MiddleName;
            customersNewToPass.LastName = customerRegister.LastName;
            customersNewToPass.Suffix = customerRegister.Suffix;
            customersNewToPass.CompanyName = customerRegister.CompanyName;
            customersNewToPass.SalesPerson = customerRegister.SalesPerson;
            customersNewToPass.EmailAddress = EncryptionSHA256.sha256Encrypt(customerRegister.EmailAddress);
            customersNewToPass.Phone = customerRegister.Phone;
            customersNewToPass.Rowguid = customerRegister.Rowguid;
            customersNewToPass.ModifiedDate = customerRegister.ModifiedDate;
            customersNewToPass.Role = customerRegister.Role;

            RegisterLogic registerLogic = new RegisterLogic(_context);



            registerLogic.PostCustomerNew(customersNewToPass);
            if (registerLogic.PostCredentials(credentialToPass))
            {
                int customerId = dbUtilityLT2019.SelectIdCCustomerNew(customersNewToPass.EmailAddress);

                dbUtilityCredentials.UpdateCredentialId(customersNewToPass, customerId);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
