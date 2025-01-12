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
    public class operationsController : ControllerBase
    {
        private readonly Operations _operations;
        private readonly ApplicationDbContext _context;

        public operationsController(ApplicationDbContext context)
        {
            _context = context;
            _operations = new Operations(_context);
        }



        [HttpPost("{customerId}/deposit")]
        public IActionResult Deposit(int customerId, [FromBody] double amount)
        {
            try
            {
                _operations.SetCustomer(customerId);
                _operations.Deposit(amount);
                return Ok("Deposit successful.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{customerId}/withdraw")]
        public IActionResult Withdraw(int customerId, [FromBody] double amount)
        {
            try
            {
                _operations.SetCustomer(customerId);
                _operations.Withdrawal(amount);
                return Ok("Withdrawal successful.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{customerId}/balance")]
        public IActionResult GetBalance(int customerId)
        {
            try
            {
                _operations.SetCustomer(customerId);
                var balance = _operations.BalanceInquiry();
                return Ok(new { Balance = balance });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{customerId}/sendMoney")]
        public IActionResult SendMoney(int customerId, [FromBody] string ReceiverName, double Amount)
        {
            try
            {
                _operations.SetCustomer(customerId);
                _operations.SendMoney(ReceiverName,Amount);

                return Ok($"Money sent successfully to {ReceiverName}. Remaining balance: {_operations.BalanceInquiry()}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
     
        // Endpoint to receive money
        [HttpPost("{customerId}/receiveMoney")]
        public IActionResult ReceiveMoney(int customerId)
        {
            try
            {
                _operations.SetCustomer(customerId);
                var message = _operations.ReceiveMoney();
                return Ok(new { Message = message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        // Endpoint to get pending transactions
        [HttpGet("{customerId}/pendingTransactions")]
        public IActionResult GetPendingTransactions(int customerId)
        {
            try
            {
                _operations.SetCustomer(customerId);
                var transactions = _operations.DisplayPendingTransactions();
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        // Endpoint to get completed transactions
        [HttpGet("{customerId}/completedTransactions")]
        public IActionResult GetCompletedTransactions(int customerId)
        {
            try
            {
                _operations.SetCustomer(customerId);
                var transactions = _operations.DisplayMadeTransactions();
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

    }

}

