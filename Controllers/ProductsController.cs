using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BetaCycle4.Models;
using Microsoft.AspNetCore.Authorization;
using BetaCycle4.Logger;

namespace BetaCycle4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DbTracer _dbTracer;
        private readonly AdventureWorksLt2019Context _context;
        private int lastId = 0;

        public ProductsController(AdventureWorksLt2019Context context, DbTracer dbTracer)
        {
            _context = context;
            _dbTracer = dbTracer;
        }

        [Route("GetProductsByPage")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByPage()
        {
            int rowPage = 12;

            var page = await _context.Products.FromSql($"SELECT TOP 12 * FROM [SalesLT].[Product] ORDER BY ProductID DESC")
                .Take( rowPage )
                .ToListAsync();
            return page;
        }

        //[Route("GetProductsByPage")]
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory()
        //{
        //    int rowPage = 12;

        //    var page = await _context.Products.FromSql($"SELECT TOP 12 * FROM [SalesLT].[Product] ORDER BY ProductID DESC")
        //        .Take(rowPage)
        //        .ToListAsync();
        //    return page;
        //}

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ProductExists(id))
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

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        [Authorize]

        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
