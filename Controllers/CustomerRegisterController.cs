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
            try
            {
                CustomersNewController customersNewController = new CustomersNewController(_context, _config, _emailService);

                Credentials credentialToPass = new();
                CustomerNew customersNewToPass = new();
                CustomerAddress customerAddressToPass = new();
                Address addressToPass = new();

                //CREDENTIALS
                credentialToPass.EmailAddress = customerRegister.EmailAddress;
                credentialToPass.Password = customerRegister.Password;
                //

                //CUSTOMER NEW
                customersNewToPass.FirstName = customerRegister.FirstName;
                customersNewToPass.MiddleName = customerRegister.MiddleName;
                customersNewToPass.LastName = customerRegister.LastName;
                customersNewToPass.EmailAddress = EncryptionSHA256.sha256Encrypt(customerRegister.EmailAddress);
                customersNewToPass.Phone = customerRegister.Phone;
                customersNewToPass.ModifiedDate = customerRegister.ModifiedDate.AddHours(2);
                //

                //ADDRESS
                addressToPass.AddressLine1 = customerRegister.AddressLine1;
                addressToPass.AddressLine2 = customerRegister.AddressLine2;
                addressToPass.City = customerRegister.City;
                addressToPass.StateProvince = customerRegister.StateProvince;
                addressToPass.CountryRegion = customerRegister.CountryRegion;
                addressToPass.PostalCode = customerRegister.PostalCode;
                addressToPass.ModifiedDate = customerRegister.ModifiedDate.AddHours(2);
                //

                //CUSTOMER ADDRESS           
                customerAddressToPass.AddressType = customerRegister.AddressType;
                customerAddressToPass.ModifiedDate = customerRegister.ModifiedDate.AddHours(2);
                //

                RegisterLogic registerLogic = new RegisterLogic(_context);

                //INSERT CREDENTIALS
                if (registerLogic.PostCredentials(credentialToPass))
                {
                    //INSERT CUSTOMER NEW
                    if (registerLogic.PostCustomerNew(customersNewToPass))
                    {
                        int customerId = dbUtilityLT2019.SelectIdCustomerNew(customersNewToPass.EmailAddress);
                        dbUtilityCredentials.UpdateCredentialId(customersNewToPass, customerId);

                        //set email customerNew null
                        customersNewToPass.CustomerId = customerId;
                        customersNewToPass.EmailAddress = null;
                        registerLogic.SetEmailNull(customerId,customersNewToPass);
                        //

                        //CUSTOMER ID in ADDRESS
                        addressToPass.CustomerId = customerId;
                        //

                        //INSERT ADDRESS NEW 
                        if (dbUtilityLT2019.PostAddressNew(addressToPass) == 1)
                        {
                            int addressId = dbUtilityLT2019.SelectAddressId(customerId);

                            //CUSTOMER ADDRESS
                            customerAddressToPass.CustomerId = customerId;
                            customerAddressToPass.AddressId = addressId;
                            //

                            //INSERT CUSTOMER ADDRESS
                            if (dbUtilityLT2019.PostCustomerAddressNew(customerAddressToPass) == 1)
                            {
                                return Ok();
                            }
                            else
                            {
                                return BadRequest("Errore in post customerAddress");
                            }
                        }
                        else
                        {                      
                            return BadRequest("PostAddress NON è andata a buon fine");
                        }
                    }
                    else
                    {
                        dbUtilityCredentials.DeleteCredentials(0);
                        return BadRequest("ERROR IN CustomerNew");
                    }
                }
                else
                {
                    return BadRequest("ERROR IN Credentials");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
                
            }
        }
    }
}
