using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ATM_Api.Data;

namespace ATM_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionInfoesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TransactionInfoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/TransactionInfoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionInfo>>> Gettransactions()
        {
            return await _context.transactions.ToListAsync();
        }

        // GET: api/TransactionInfoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionInfo>> GetTransactionInfo(int id)
        {
            var transactionInfo = await _context.transactions.FindAsync(id);

            if (transactionInfo == null)
            {
                return NotFound();
            }

            return transactionInfo;
        }

        // PUT: api/TransactionInfoes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransactionInfo(int id, TransactionInfo transactionInfo)
        {
            if (id != transactionInfo.TransId)
            {
                return BadRequest();
            }

            _context.Entry(transactionInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionInfoExists(id))
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

        // POST: api/TransactionInfoes
        [HttpPost]
        public async Task<ActionResult<TransactionInfo>> PostTransactionInfo(TransactionInfo transactionInfo)
        {
            _context.transactions.Add(transactionInfo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransactionInfo", new { id = transactionInfo.TransId }, transactionInfo);
        }

        // DELETE: api/TransactionInfoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransactionInfo(int id)
        {
            var transactionInfo = await _context.transactions.FindAsync(id);
            if (transactionInfo == null)
            {
                return NotFound();
            }

            _context.transactions.Remove(transactionInfo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TransactionInfoExists(int id)
        {
            return _context.transactions.Any(e => e.TransId == id);
        }
    }
}
