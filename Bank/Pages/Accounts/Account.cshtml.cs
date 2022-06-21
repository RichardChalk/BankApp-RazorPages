using AutoMapper;
using Bank.Models;
using Bank.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Bank.Pages.Accounts
{
    [Authorize(Roles = "Cashier")]
    public class AccountModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public string SortOrder { get; set; }
        public string SortColumn { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public string SearchText { get; set; }


        public AccountModel(ICustomerService customerService, IAccountService accountService, IMapper mapper)
        {
            _customerService = customerService;
            _accountService = accountService;
            _mapper = mapper;
        }


        public AccountProfileViewModel AccountProfile { get; set; } = new AccountProfileViewModel();
        public Account AccountFromHtml { get; set; }



        public class AccountProfileViewModel
        {

            public int CustomerId { get; set; }
            public int Id { get; set; }

            [MaxLength(70)]
            public string Frequency { get; set; } = null!;

            public DateTime Created { get; set; }

            [MaxLength(70)]
            public decimal Balance { get; set; }

            public ICollection<Transaction> Transactions { get; set; }
        }

        public class TransactionViewModel
        {
            public int TransactionId { get; set; }
            public DateTime Date { get; set; }
            public decimal Amount { get; set; }
            public decimal Balance { get; set; }

        }


        //[Authorize(Roles="Cashier")]
        public void OnGet(int accountId, string sortColumn, string sortOrder, string searchText, int pageNo, int customerId)
        {
            SearchText = searchText;
            SortOrder = sortOrder;
            SortColumn = sortColumn;
            if (pageNo == 0)
                pageNo = 1;
            CurrentPage = pageNo;

            // Collect customer account
            AccountFromHtml = _accountService.GetCustomerAccount(accountId);

            // Automapper automatically maps all properties with the SAME NAME!
            AccountProfile = _mapper.Map<AccountProfileViewModel>(AccountFromHtml);

            AccountProfile.CustomerId = customerId;
            // We dont need these lines thanks to Automapper!
            //AccountProfile.Id = AccountFromHtml.AccountId;
            //AccountProfile.Frequency = AccountFromHtml.Frequency;
            //AccountProfile.Created = AccountFromHtml.Created;
            //AccountProfile.Balance = AccountFromHtml.Balance;
            AccountProfile.Transactions = _accountService.GetTransactions(accountId)
                .OrderByDescending(t => t.Date)
                .ToList();
        }

        public IActionResult OnGetFetchMore(int accountId, long lastTicks)
        {
            DateTime dateOfLastShown = new DateTime(lastTicks).AddMilliseconds(100);

            var listOfTransactions = _accountService.GetTransactions(accountId)
                .Where(t => lastTicks == 0 || t.Date > dateOfLastShown)
                .OrderByDescending(t => t.Date)

                .Take(10)
                .ToList();

            // Using automapper we can now change our list of "Transactions"....
            // To a list of "listOfTransactions"
            var listOfTransactionsViewModel = _mapper.Map<List<TransactionViewModel>>(listOfTransactions);



            if (listOfTransactionsViewModel.Any())
                lastTicks = listOfTransactionsViewModel.Last().Date.Ticks;
            return new JsonResult(new { items = listOfTransactionsViewModel, lastTicks });
        }
    }
}
