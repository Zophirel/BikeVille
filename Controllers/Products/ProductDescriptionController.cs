using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BikeVille.Models.Products;
using BikeVille.SqlDbContext;

namespace BikeVille.Controllers.Products
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductDescriptionController(BikeVilleProductsContext context) : ControllerBase
    {
        private readonly BikeVilleProductsContext _context = context;

        // GET: api/ProductDescription
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDescription>>> GetProductDescriptions()
        {
            return await _context.ProductDescriptions.ToListAsync();
        }

        // GET: api/ProductDescription/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDescription>> GetProductDescription(int id)
        {
            var productDescription = await _context.ProductDescriptions.FindAsync(id);

            if (productDescription == null)
            {
                return NotFound();
            }

            return productDescription;
        }

        // PUT: api/ProductDescription/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductDescription(int id, ProductDescription productDescription)
        {
            if (id != productDescription.ProductDescriptionId)
            {
                return BadRequest();
            }

            _context.Entry(productDescription).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductDescriptionExists(id))
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

        // POST: api/ProductDescription
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductDescription>> PostProductDescription(ProductDescription productDescription)
        {
            _context.ProductDescriptions.Add(productDescription);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductDescription", new { id = productDescription.ProductDescriptionId }, productDescription);
        }

        // DELETE: api/ProductDescription/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductDescription(int id)
        {
            var productDescription = await _context.ProductDescriptions.FindAsync(id);
            if (productDescription == null)
            {
                return NotFound();
            }

            _context.ProductDescriptions.Remove(productDescription);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductDescriptionExists(int id)
        {
            return _context.ProductDescriptions.Any(e => e.ProductDescriptionId == id);
        }
    }
}
