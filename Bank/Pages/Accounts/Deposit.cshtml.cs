using Bank.Models;
using Bank.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Pages.Accounts
{
    [Authorize(Roles = "Cashier")]
    public class DepositModel : PageModel
    {
        private readonly IAccountService _accountService;

        public DepositViewModel Deposit { get; set; }

        [BindProperty]
        [Required]
        [Range(1, 1000000, ErrorMessage = "Please choose a number between 1 och 1000000")]
        [Column(TypeName = "decimal(13, 2)")]
        public decimal Amount { get; set; }

        public DepositModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public class DepositViewModel
        {
            public Account Account { get; set; }

            [Range(1, 1000000)]
            public decimal Amount { get; set; }

            public int CustomerId { get; set; }
        }


        public void OnGet(int accountId, int customerId)
        {
            Deposit = _accountService.GetNewDepositViewModel();
            Deposit.Account = _accountService.GetCustomerAccount(accountId);
            Deposit.Amount = 0;
            Deposit.CustomerId = customerId;
        }

        public IActionResult OnPost(int accountId, int customerId)
        {
            if (ModelState.IsValid)
            {
                var account = _accountService.GetCustomerAccount(accountId);
                _accountService.MakeDeposit(account, Amount);

                // Tempdata is used together with toastr to display verification message
                TempData["success"] = "Deposit made successfully!";

                return RedirectToPage("/Customers/Customer", new { customerId = customerId });
            }

            // I have to fill "Deposit" even if model is invalid
            // If I don't I can't show Balance and account Id in HTML 
            Deposit = _accountService.GetNewDepositViewModel();
            Deposit.Account = _accountService.GetCustomerAccount(accountId);
            Deposit.Amount = 0;
            Deposit.CustomerId = customerId;

            return Page();
        }
    }
}
