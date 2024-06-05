using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BetaCycle4.Models;
using SqlManager.BLogic;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.Security.Cryptography;
using BetaCycle4.Logic;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Microsoft.AspNetCore.Authorization;
using BetaCycle4.Logger;

namespace BetaCycle4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersNewController : ControllerBase
    {
        private readonly AdventureWorksLt2019Context _context;
        private readonly DbTracer _dbTracer;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;

        public CustomersNewController(AdventureWorksLt2019Context context, IConfiguration config, IEmailService emailService, DbTracer dbTracer)
        {
            _context = context;
            _config = config;
            _emailService = emailService;
            _dbTracer = dbTracer;
        }

        // GET: api/CustomerNews
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerNew>>> GetCustomerNews()
        {
            return await _context.CustomerNews.ToListAsync();
        }

        // GET: api/CustomerNews/5
        [Authorize]

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerNew>> GetCustomerNew(int id)
        {
            var customerNew = await _context.CustomerNews.FindAsync(id);

            if (customerNew == null)
            {
                return NotFound();
            }

            return customerNew;
        }

        // PUT: api/CustomerNews/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerNew(int id, CustomerNew customerNew)
        {
            if (id != customerNew.CustomerId)
            {
                return BadRequest();
            }

            _context.Entry(customerNew).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!CustomerNewExists(id))
                {
                    return NotFound();
                }
                else
                {
                    _dbTracer.InsertError(ex.Message, ex.HResult, ex.StackTrace);
                }
            }

            return NoContent();
        }

        // POST: api/CustomerNews
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]

        [HttpPost] 
        public async Task<ActionResult<CustomerNew>> PostCustomerNew(CustomerNew customerNew)
        {
            
            _context.CustomerNews.Add(customerNew);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomerNew", new { id = customerNew.CustomerId }, customerNew);
        }

        // DELETE: api/CustomerNews/5
        //[Authorize]

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteCustomerNew(int id)
        //{
        //    var customerNew = await _context.CustomerNews.FindAsync(id);
        //    if (customerNew == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.CustomerNews.Remove(customerNew);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool CustomerNewExists(int id)
        {
            return _context.CustomerNews.Any(e => e.CustomerId == id);
        }

        

        [HttpPost("send-reset-email/{email}")]
        public async Task<IActionResult> sendEmail(string email)
        {
            // it should be done into credentials
            DbUtility dbUtilityCredentials = new("Data Source=.\\SQLEXPRESS;Initial Catalog=CustomerCredentials;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            CredentialDB credentialDB = new CredentialDB();
            credentialDB = dbUtilityCredentials.credentialsFromEmail(email);
            
            if (credentialDB is null)
            {
                return NotFound(new
                {
                    statuscode = 404,
                    message = "email does not exist"
                });
            }
            // return the credentials of that user with that email


            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var emailToken = Convert.ToBase64String(tokenBytes);
            credentialDB.ResetPasswordToken = emailToken;
            credentialDB.ResetPasswordExpiry = DateTime.Now.AddMinutes(15);
            string from = _config["EmailSettings:From"];
            var emailModel = new EmailModel(email, "Reset Password", EmailBody.EmailStringBody(email, emailToken));
            _emailService.sendEmail(emailModel);
            //_context.Entry(credentialDB).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
            dbUtilityCredentials.writeInfoOnDb(credentialDB);
            return Ok(new
            {
                StatusCode = 200,
                Message = "Email sent"
            });

            // expand for old code
            {
                //var user = await _context.CustomerNews.FirstOrDefaultAsync(a => a.Email == email);
                //if (user is null)
                //{
                //    return NotFound(new
                //    {
                //        StatusCode = 404,
                //        Message = "email does not exist"
                //    }) ;
                //}
                //var tokenBytes = RandomNumberGenerator.GetBytes(64);
                //var emailToken = Convert.ToBase64String(tokenBytes);
                //user.ResetPasswordToken = emailToken;
                //user.ResetPasswordExpiry = DateTime.Now.AddMinutes(15);
                //string from = _config["EmailSettings:From"];
                //var emailModel = new EmailModel(email, "Reset Password", EmailBody.EmailStringBody(email, emailToken));
                //_emailService.sendEmail(emailModel);
                //_context.Entry(user).State = EntityState.Modified;
                //await _context.SaveChangesAsync();
                //return Ok(new
                //{
                //    StatusCode = 200,
                //    Message = "Email sent"
                //});
            }

        }











        //var newToken = resetPassword.EmailToken.Replace(" ", "+");
        //var customer = await _context.CustomerNews.AsNoTracking().FirstOrDefaultAsync(a => a.Email == resetPassword.Email);
        //if (customer is null)
        //{
        //    return NotFound(new
        //    {
        //        StatusCode = 404,
        //        Message = "Customer does not exist"
        //    });
        //}


        [HttpPost("reset-password")]
        public async Task<IActionResult> resetPassword(ResetPassword resetPassword)
        {
            DbUtility dbUtilityCredentials = new("Data Source=.\\SQLEXPRESS;Initial Catalog=CustomerCredentials;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            CredentialDB credentialDB = new CredentialDB();
            
            credentialDB = dbUtilityCredentials.credentialsFromEmail(resetPassword.Email);

            if (credentialDB is null)
            {
                return NotFound(new
                {
                    statuscode = 404,
                    message = "email does not exist"
                });
            }


            

            var tokenCode = credentialDB.ResetPasswordToken;           
            DateTime emailTokenExpiry = (DateTime)credentialDB.ResetPasswordExpiry;
            if (tokenCode != resetPassword.EmailToken || emailTokenExpiry < DateTime.Now)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    message = "Token expired"
                });
            }
            // customer.Password = cript password
            //_context.Entry(customer).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
            dbUtilityCredentials.writeNewPasswordOnDb(resetPassword);
            return Ok(new
            {
                StatusCode = 200,
                Message = "Password reset successfully"
            });
        }
    }
}
