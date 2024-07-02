using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BankovniRestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private static readonly List<Transaction> Transactions = new List<Transaction>
        {
            new Transaction { Id = 1, Amount = 100, Description = "Deposit" },
            new Transaction { Id = 2, Amount = 50, Description = "Withdrawal" }
        };

        [HttpGet]
        public IEnumerable<Transaction> GetTransactions()
        {
            return Transactions;
        }

        [HttpPost]
        public IActionResult CreateTransaction([FromBody] Transaction transaction)
        {
            transaction.Id = Transactions.Count + 1;
            Transactions.Add(transaction);
            return CreatedAtAction(nameof(GetTransactions), new { id = transaction.Id }, transaction);
        }
    }

    public class Transaction
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }
    }
}
