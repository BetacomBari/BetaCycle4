using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BetaCycle4.Models;
using Microsoft.Data.SqlClient;
using BetaCycle4.Logic;

namespace BetaCycle4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartsController : ControllerBase
    {
        private readonly AdventureWorksLt2019Context _context;
        DbUtilityForCart _cartUtility = new("Data Source=.\\SQLEXPRESS;Initial Catalog=AdventureWorksLT2019;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        DbUtilityForCart _cartUtilityCredentials = new("Data Source=.\\SQLEXPRESS;Initial Catalog=CustomerCredentials;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

        public ShoppingCartsController(AdventureWorksLt2019Context context)
        {
            _context = context;
        }

        // GET: api/ShoppingCarts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShoppingCart>>> GetShoppingCart()
        {
            return await _context.ShoppingCart.ToListAsync();
        }

        ////GET: api/ShoppingCarts/5
        [HttpGet("{email}/1")]
        public  ActionResult<int> GetIdByEmail(string email)
        {
            return _cartUtilityCredentials.SelectIdCustomerNew(email);
        }

        
        
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ShoppingCart>>> GetShoppingCart(int id)
        {
            //var shoppingCart = await _context.ShoppingCart.FromSql($"SELECT * FROM [dbo].[ShoppingCart] WHERE CustomerID = {id}").ToListAsync();

            //if (shoppingCart == null)
            //{
            //    return NotFound();
            //}
            var parameter = new SqlParameter("id", id);
            return await _context.ShoppingCart.FromSqlRaw($"SELECT * FROM [dbo].[ShoppingCart] WHERE CustomerID = @id", parameter).ToListAsync();
        }

        [HttpGet("{id_user}/fromIdToCnn")]
        public ActionResult<int> GetCnnIdById(int id_user)
        {
            return _cartUtilityCredentials.fromIdToCnnId(id_user);
        }

        [HttpGet("{id_user}/last")]
        public ActionResult<int> GetLastItemBoughtFromUser(int id_user)
        {
            return _cartUtility.lastItemBoughtByCustomerId(id_user);
        }

        // PUT: api/ShoppingCarts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShoppingCart(int id, ShoppingCart shoppingCart)
        {
            if (id != shoppingCart.ShoppingId)
            {
                return BadRequest();
            }

            _context.Entry(shoppingCart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShoppingCartExists(id))
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

        // POST: api/ShoppingCarts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> PostShoppingCart(ShoppingCart shoppingCart)
        {
            _context.ShoppingCart.Add(shoppingCart);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShoppingCart", new { id = shoppingCart.ShoppingId }, shoppingCart);
        }

        // DELETE: api/ShoppingCarts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShoppingCart(int id)
        {
            var shoppingCart = await _context.ShoppingCart.FindAsync(id);
            if (shoppingCart == null)
            {
                return NotFound();
            }

            _context.ShoppingCart.Remove(shoppingCart);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ShoppingCartExists(int id)
        {
            return _context.ShoppingCart.Any(e => e.ShoppingId == id);
        }
    }
}
