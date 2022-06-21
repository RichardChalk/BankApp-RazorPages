using AutoMapper;
using Bank.Models;
using Bank.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BankAPI.Controllers
{
    //[Route("api/[controller]")]
    [Produces("application/json")]
    [Route("api")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        public TransactionsController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        public AccountProfileViewModel AccountProfile { get; set; } = new AccountProfileViewModel();
        public Account AccountChosen { get; set; }

        public class AccountProfileViewModel
        {
            public int CustomerId { get; set; }
            public int Id { get; set; }

            [MaxLength(70)]
            public string Frequency { get; set; } = null!;

            public DateTime Created { get; set; }

            [MaxLength(70)]
            public decimal Balance { get; set; }

            public ICollection<TransactionViewModel> Transactions { get; set; }
        }

        public class TransactionViewModel
        {
            public int TransactionId { get; set; }
            public DateTime Date { get; set; }
            public decimal Amount { get; set; }
            public decimal Balance { get; set; }

        }




        // READ ONE ///////////////////////////////////////////////////////
        /// <summary>
        /// Retrieve a SPECIFIC account from the database
        /// </summary>
        /// <param name="id">
        /// Id of specific account
        /// </param>
        /// <returns>
        /// The chosen account (by Id)
        /// </returns>
        /// <remarks>
        /// Example end point: GET /api/account/1
        /// </remarks>
        /// <response code="200">
        /// Successfully returned the chosen account (by Id)
        /// </response>
        [HttpGet("account/{id}/{limit}/{offset}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        //[Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetAccount(int id, int limit, int offset)
        {
            // Collect customer account
            AccountChosen = _accountService.GetCustomerAccount(id);

            // Automapper automatically maps all properties with the SAME NAME!
            AccountProfile = _mapper.Map<AccountProfileViewModel>(AccountChosen);

            AccountProfile.CustomerId = id;

            AccountProfile.Transactions = _accountService.GetTransactions(id)
                .OrderByDescending(t => t.Date)
                .Take(limit)
                .Skip(offset)
                .Select(t => new TransactionViewModel
                {
                    TransactionId = t.TransactionId,
                    Amount = t.Amount,
                    Balance = t.Balance,
                    Date = t.Date
                })
                .ToList();

            return Ok(AccountProfile.Transactions);
        }
    }
}
