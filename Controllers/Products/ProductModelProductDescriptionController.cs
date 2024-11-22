using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BikeVille.Models.Products;
using BikeVille.SqlDbContext;

namespace BikeVille.Controllers.Products
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductModelProductDescriptionController(BikeVilleProductsContext context) : ControllerBase
    {
        private readonly BikeVilleProductsContext _context = context;

        // GET: api/ProductModelProductDescription
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductModelProductDescription>>> GetProductModelProductDescriptions()
        {
            return await _context.ProductModelProductDescriptions.ToListAsync();
        }

        // GET: api/ProductModelProductDescription/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductModelProductDescription>> GetProductModelProductDescription(int id)
        {
            var productModelProductDescription = await _context.ProductModelProductDescriptions.FindAsync(id);

            if (productModelProductDescription == null)
            {
                return NotFound();
            }

            return productModelProductDescription;
        }

        // PUT: api/ProductModelProductDescription/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductModelProductDescription(int id, ProductModelProductDescription productModelProductDescription)
        {
            if (id != productModelProductDescription.ProductModelId)
            {
                return BadRequest();
            }

            _context.Entry(productModelProductDescription).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductModelProductDescriptionExists(id))
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

        // POST: api/ProductModelProductDescription
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductModelProductDescription>> PostProductModelProductDescription(ProductModelProductDescription productModelProductDescription)
        {
            _context.ProductModelProductDescriptions.Add(productModelProductDescription);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProductModelProductDescriptionExists(productModelProductDescription.ProductModelId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProductModelProductDescription", new { id = productModelProductDescription.ProductModelId }, productModelProductDescription);
        }

        // DELETE: api/ProductModelProductDescription/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductModelProductDescription(int id)
        {
            var productModelProductDescription = await _context.ProductModelProductDescriptions.FindAsync(id);
            if (productModelProductDescription == null)
            {
                return NotFound();
            }

            _context.ProductModelProductDescriptions.Remove(productModelProductDescription);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductModelProductDescriptionExists(int id)
        {
            return _context.ProductModelProductDescriptions.Any(e => e.ProductModelId == id);
        }
    }
}
