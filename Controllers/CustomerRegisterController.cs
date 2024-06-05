using BetaCycle4.Logger;
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
    [Route("Register")]
    [ApiController]
    public class CustomerRegisterController : ControllerBase
    {
        DbUtility dbUtilityLT2019 = new("Data Source=.\\SQLEXPRESS;Initial Catalog=AdventureWorksLT2019;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        DbUtility dbUtilityCredentials = new("Data Source=.\\SQLEXPRESS;Initial Catalog=CustomerCredentials;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

        private readonly AdventureWorksLt2019Context _context;
        private readonly DbTracer _dbTracer;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;

        public CustomerRegisterController(AdventureWorksLt2019Context context, DbTracer dbTracer)
        {
            _dbTracer = dbTracer;
            _context = context;
        }

        [HttpPost]
        public IActionResult Register(CustomerRegister customerRegister)
        {
            RegisterLogic registerLogic = new RegisterLogic(_context);
            int customerId = 0;
            bool emailExistInDbADLT2019 = false;

            try
            {
                CustomersNewController customersNewController = new CustomersNewController(_context, _config, _emailService, _dbTracer);

                #region VERIFICHE CAMPI
                //VERIFY CUSTOMER NEW
                if (!LogicVerify.IsValidEmail(customerRegister.EmailAddress))
                {
                    return BadRequest("EMAIL ERROR");
                }

                if (!LogicVerify.VerifyLength(customerRegister.FirstName, 50, false))
                {
                    return BadRequest("FirstName ERROR");
                }

                if (!LogicVerify.VerifyLength(customerRegister.MiddleName, 50, true))
                {
                    return BadRequest("MiddleName ERROR");
                }

                if (!LogicVerify.VerifyLength(customerRegister.LastName, 50, false))
                {
                    return BadRequest("LastName ERROR");
                }

                if (!LogicVerify.VerifyLength(customerRegister.Phone, 25, false))
                {
                    return BadRequest("Phone ERROR");
                }

                //VERIFY PASSWORD          
                if (!LogicVerify.IsValidPassword(customerRegister.Password))
                {
                    return BadRequest("Password ERROR");
                }

                //VERIFY ADDRESS NEW          
                if (!LogicVerify.VerifyLength(customerRegister.AddressLine1, 60, false))
                {
                    return BadRequest("AddressLine1 ERROR");
                }

                if (!LogicVerify.VerifyLength(customerRegister.AddressLine2, 60, true))
                {
                    return BadRequest("AddressLine2 ERROR");
                }
                else
                {
                    customerRegister.AddressLine2 = "";
                }

                if (!LogicVerify.VerifyLength(customerRegister.City, 30, false))
                {
                    return BadRequest("City ERROR");
                }

                if (!LogicVerify.VerifyLength(customerRegister.StateProvince, 50, false))
                {
                    return BadRequest("StateProvince ERROR");
                }

                if (!LogicVerify.VerifyLength(customerRegister.CountryRegion, 50, false))
                {
                    return BadRequest("CountryRegion ERROR");
                }

                if (!LogicVerify.VerifyLength(customerRegister.PostalCode, 15, false))
                {
                    return BadRequest("PostalCode ERROR");
                }

                //VERIFY CUSTOMER ADDRESS
                if (!LogicVerify.VerifyLength(customerRegister.AddressType, 50, false))
                {
                    return BadRequest("AddressType ERROR");
                }
                #endregion

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
                customersNewToPass.ModifiedDate = DateTime.Now;
                //

                //ADDRESS
                addressToPass.AddressLine1 = customerRegister.AddressLine1;
                addressToPass.AddressLine2 = customerRegister.AddressLine2;
                addressToPass.City = customerRegister.City;
                addressToPass.StateProvince = customerRegister.StateProvince;
                addressToPass.CountryRegion = customerRegister.CountryRegion;
                addressToPass.PostalCode = customerRegister.PostalCode;
                addressToPass.ModifiedDate = DateTime.Now;
                //

                //CUSTOMER ADDRESS           
                customerAddressToPass.AddressType = customerRegister.AddressType;
                customerAddressToPass.ModifiedDate = DateTime.Now;
                //

                //INSERT CREDENTIALS
                if (registerLogic.PostCredentials(credentialToPass))
                {
                    //INSERT CUSTOMER NEW
                    if (registerLogic.PostCustomerNew(customersNewToPass))
                    {
                        customerId = dbUtilityLT2019.SelectIdCustomerNew(customersNewToPass.EmailAddress);
                        dbUtilityCredentials.UpdateCredentialId(customersNewToPass, customerId);

                        //set email customerNew null
                        customersNewToPass.CustomerId = customerId;
                        customersNewToPass.EmailAddress = null;
                        registerLogic.SetEmailNull(customerId, customersNewToPass);
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
                                emailExistInDbADLT2019 = dbUtilityLT2019.CheckEmailDbAWLT2019(customerRegister.EmailAddress);
                                if (emailExistInDbADLT2019 && !dbUtilityLT2019.CheckIsElseWhere(customerRegister.EmailAddress))
                                {
                                    dbUtilityLT2019.SetIsElseWhereTrue(customerRegister.EmailAddress);
                                }
                                return Ok(new { message = "registrazione completa" });
                            }
                            else
                            {
                                dbUtilityCredentials.DeleteCredentials(customerId);
                                dbUtilityLT2019.DeleteAddressNew(addressId);
                                registerLogic.DeleteCustomerNew(customerId);
                                return BadRequest("Errore in post customerAddress");
                            }
                        }
                        else
                        {
                            dbUtilityCredentials.DeleteCredentials(customerId);
                            registerLogic.DeleteCustomerNew(customerId);
                            return BadRequest("PostAddress NON è andata a buon fine");
                        }
                    }
                    else
                    {
                        dbUtilityCredentials.DeleteCredentials(customerId);
                        return BadRequest("ERROR IN CustomerNew");
                    }
                }
                else
                {
                    return BadRequest(new { message = "emailExist" });
                }
            }
            catch (Exception ex)
            {
                _dbTracer.InsertError(ex.Message, ex.HResult, ex.StackTrace);
                throw;
            }
        }
    }
}
