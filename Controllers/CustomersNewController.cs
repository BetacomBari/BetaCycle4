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

namespace BetaCycle4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersNewController : ControllerBase
    {
        private readonly AdventureWorksLt2019Context _context;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;

        public CustomersNewController(AdventureWorksLt2019Context context, IConfiguration config, IEmailService emailService)
        {
            _context = context;
            _config = config;
            _emailService = emailService;
        }

        // GET: api/CustomerNews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerNew>>> GetCustomerNews()
        {
            return await _context.CustomerNews.ToListAsync();
        }

        // GET: api/CustomerNews/5
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
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerNewExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CustomerNews
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CustomerNew>> PostCustomerNew(CustomerNew customerNew)
        {
            _context.CustomerNews.Add(customerNew);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomerNew", new { id = customerNew.CustomerId }, customerNew);
        }

        // DELETE: api/CustomerNews/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerNew(int id)
        {
            var customerNew = await _context.CustomerNews.FindAsync(id);
            if (customerNew == null)
            {
                return NotFound();
            }

            _context.CustomerNews.Remove(customerNew);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerNewExists(int id)
        {
            return _context.CustomerNews.Any(e => e.CustomerId == id);
        }

        [HttpPost("send-reset-email/{email}")]
        public async Task<IActionResult> sendEmail(string email)
        {
            // it should be done into credentials
            var user = await _context.CustomerNews.FirstOrDefaultAsync(a => a.Email == email);
            if (user is null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "email does not exist"
                }) ;
            }
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var emailToken = Convert.ToBase64String(tokenBytes);
            user.ResetPasswordToken = emailToken;
            user.ResetPasswordExpiry = DateTime.Now.AddMinutes(15);
            string from = _config["EmailSettings:From"];
            var emailModel = new EmailModel(email, "Reset Password", EmailBody.EmailStringBody(email, emailToken));
            _emailService.sendEmail(emailModel);
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Email sent"
            });
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> resetPassword(ResetPassword resetPassword)
        {
            var newToken = resetPassword.EmailToken.Replace(" ", "+");
            var customer = await _context.CustomerNews.AsNoTracking().FirstOrDefaultAsync(a => a.Email == resetPassword.Email);
            if (customer is null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Customer does not exist"
                });
            }
            var tokenCode = customer.ResetPasswordToken;
            DateTime emailTokenExpiry = customer.ResetPasswordExpiry;
            if (tokenCode != resetPassword.EmailToken || emailTokenExpiry< DateTime.Now)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    message = "Token expired"
                });
            }
            // customer.Password = cript password
            _context.Entry(customer).State = EntityState.Modified;  
            await _context.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Password reset successfully"
            });
        }
    }
}
