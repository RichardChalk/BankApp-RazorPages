using Bank.Models;
using Bank.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Pages.Accounts
{
    [Authorize(Roles = "Cashier")]
    public class WithdrawalModel : PageModel
    {
        private readonly IAccountService _accountService;

        public WithdrawalViewModel Withdrawal { get; set; }


        [BindProperty]
        [Required]
        [Range(1, 1000000, ErrorMessage = "Please choose a number between 1 och 1000000")]
        [Column(TypeName = "decimal(13, 2)")]
        public decimal Amount { get; set; }

        public WithdrawalModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public class WithdrawalViewModel
        {
            public Account Account { get; set; }

            [Range(1, 1000000)]
            public decimal Amount { get; set; }

            public int CustomerId { get; set; }
        }


        public void OnGet(int accountId, int customerId)
        {
            Withdrawal = _accountService.GetNewWithdrawalViewModel();
            Withdrawal.Account = _accountService.GetCustomerAccount(accountId);
            Withdrawal.Amount = 0;
            Withdrawal.CustomerId = customerId;
        }

        public IActionResult OnPost(int accountId, int customerId)
        {
            var account = _accountService.GetCustomerAccount(accountId);
            if (account.Balance < Amount)
            {
                ModelState.AddModelError("Amount", "Sorry, amount is too high!");
            }

            if (ModelState.IsValid)
            {
                _accountService.MakeWithdrawal(account, Amount);

                // Tempdata is used together with toastr to display verification message
                TempData["success"] = "Withdrawal made successfully!";

                return RedirectToPage("/Customers/Customer", new { customerId = customerId });
            }

            // I have to fill "Withdrawal" even if model is invalid
            // If I don't I can't show Balance and account Id in HTML 
            Withdrawal = _accountService.GetNewWithdrawalViewModel();
            Withdrawal.Account = _accountService.GetCustomerAccount(accountId);
            Withdrawal.Amount = 0;
            Withdrawal.CustomerId = customerId;

            return Page();
        }
    }
}
