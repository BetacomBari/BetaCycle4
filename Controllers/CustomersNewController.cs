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

namespace BetaCycle4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersNewController : ControllerBase
    {
        private readonly AdventureWorksLt2019Context _context;

        public CustomersNewController(AdventureWorksLt2019Context context)
        {
            _context = context;
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
        public async Task<ActionResult<CustomerNew>> PostCustomerNew(CustomerNew customerNew, Credentials credential)
        {
            _context.CustomerNews.Add(customerNew);
            await _context.SaveChangesAsync();

            //se il nome è giusto ecc.. far partire la post per le credenziali

            CredentialsController credentialsController = new CredentialsController();
            Credentials credentials = credential;


            bool isNewUser = credentialsController.PostCredentials(credentials);

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
    }
}
