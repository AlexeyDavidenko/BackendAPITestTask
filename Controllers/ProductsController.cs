using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendAPIDevelopmentTask.Data;
using BackendAPIDevelopmentTask.Models;
using BackendAPIDevelopmentTask.ViewModels;
using Microsoft.AspNetCore.Authorization;
using SimpleAuthentication.Permissions;

namespace BackendAPIDevelopmentTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDBContext _context;

        public ProductsController(AppDBContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductsIndexViewModel>>> GetProducts()
        {
            var dbProducts = await _context.Products.ToListAsync();
            var productsList = dbProducts.Select(p => new ProductsIndexViewModel(p)).ToList();
            return productsList;
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
        [Permission(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, AddEditProductViewModel product)
        {
            if (ProductExists(id))
            {
                return BadRequest("No Such product");
            }

            var dbProduct = _context.Products.FirstOrDefault(x => x.Id == id);

            dbProduct.Title = product.Title;
            dbProduct.Price = product.Price;
            dbProduct.Quantity = product.Quantity;

            _context.Entry(dbProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [Permission(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(AddEditProductViewModel product)
        {
            var dbProduct = new Product
            {
                Price = product.Price,
                Quantity = product.Quantity,
                Title = product.Title
            };

            _context.Products.Add(dbProduct);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = dbProduct.Id }, product);
        }

        // DELETE: api/Products/5
        [Authorize]
        [Permission(Roles = "Admin")]
        [HttpDelete("{id}")]
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
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
