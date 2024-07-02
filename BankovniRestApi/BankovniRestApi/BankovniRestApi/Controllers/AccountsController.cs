using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BankovniRestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private static readonly List<Account> Accounts = new List<Account>
        {
            new Account { Id = 1, Name = "John Doe", Balance = 1000 },
            new Account { Id = 2, Name = "Jane Smith", Balance = 500 }
        };

        [HttpGet]
        public IEnumerable<Account> GetAccounts()
        {
            return Accounts;
        }

        [HttpGet("{id}")]
        public ActionResult<Account> GetAccount(int id)
        {
            var account = Accounts.FirstOrDefault(a => a.Id == id);
            if (account == null) return NotFound();
            return account;
        }

        [HttpPost("transfer")]
        public IActionResult TransferFunds([FromBody] TransferRequest transferRequest)
        {
            var sourceAccount = Accounts.FirstOrDefault(a => a.Id == transferRequest.SourceAccountId);
            var destinationAccount = Accounts.FirstOrDefault(a => a.Id == transferRequest.DestinationAccountId);

            if (sourceAccount == null || destinationAccount == null) return NotFound("One or both accounts not found.");

            if (sourceAccount.Balance < transferRequest.Amount) return BadRequest("Insufficient funds.");

            sourceAccount.Balance -= transferRequest.Amount;
            destinationAccount.Balance += transferRequest.Amount;

            return Ok();
        }

    }

    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Balance { get; set; }
    }

    public class TransferRequest
    {
        public int SourceAccountId { get; set; }
        public int DestinationAccountId { get; set; }
        public double Amount { get; set; }
    }
}
