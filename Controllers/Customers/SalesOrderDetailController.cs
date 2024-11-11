using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BikeVille.Models.Customers;
using BikeVille.SqlDbContext;

namespace BikeVille.Controllers.Customers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesOrderDetailController(BikeVilleCustomersContext context) : ControllerBase
    {
        private readonly BikeVilleCustomersContext _context = context;

        // GET: api/SalesOrderDetail
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalesOrderDetail>>> GetSalesOrderDetails()
        {
            return await _context.SalesOrderDetails.ToListAsync();
        }

        // GET: api/SalesOrderDetail/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SalesOrderDetail>> GetSalesOrderDetail(int id)
        {
            var salesOrderDetail = await _context.SalesOrderDetails.FindAsync(id);

            if (salesOrderDetail == null)
            {
                return NotFound();
            }

            return salesOrderDetail;
        }

        // PUT: api/SalesOrderDetail/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSalesOrderDetail(int id, SalesOrderDetail salesOrderDetail)
        {
            if (id != salesOrderDetail.SalesOrderId)
            {
                return BadRequest();
            }

            _context.Entry(salesOrderDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalesOrderDetailExists(id))
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

        // POST: api/SalesOrderDetail
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SalesOrderDetail>> PostSalesOrderDetail(SalesOrderDetail salesOrderDetail)
        {
            _context.SalesOrderDetails.Add(salesOrderDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSalesOrderDetail", new { id = salesOrderDetail.SalesOrderId }, salesOrderDetail);
        }

        // DELETE: api/SalesOrderDetail/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalesOrderDetail(int id)
        {
            var salesOrderDetail = await _context.SalesOrderDetails.FindAsync(id);
            if (salesOrderDetail == null)
            {
                return NotFound();
            }

            _context.SalesOrderDetails.Remove(salesOrderDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SalesOrderDetailExists(int id)
        {
            return _context.SalesOrderDetails.Any(e => e.SalesOrderId == id);
        }
    }
}
