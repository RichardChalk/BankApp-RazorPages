using Bank.Models;
using Bank.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Pages.Accounts
{
    [Authorize(Roles = "Cashier")]

    public class TransferModel : PageModel
    {
        private readonly IAccountService _accountService;

        public TransferViewModel Transfer { get; set; }

        [BindProperty]
        [Required]
        [Range(1, 1000000, ErrorMessage = "Please choose a number between 1 och 1000000")]
        [Column(TypeName = "decimal(13, 2)")]
        public decimal Amount { get; set; }

        [Required]
        [BindProperty]
        [Range(1, 99999, ErrorMessage = "Please choose a valid account number")]
        public int AccountToId { get; set; }




        public TransferModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public class TransferViewModel
        {
            public Account Account { get; set; }

            public int AccountToId { get; set; }

            public decimal Amount { get; set; }

            public int CustomerId { get; set; }
        }


        public void OnGet(int accountId, int customerId)
        {
            Transfer = _accountService.GetNewTransferViewModel();
            Transfer.Account = _accountService.GetCustomerAccount(accountId);
            Transfer.Amount = 0;
            Transfer.CustomerId = customerId;
        }

        public IActionResult OnPost(int accountId, int customerId, int accountToId)
        {

            var accountTo = _accountService.GetCustomerAccount(accountToId);
            var account = _accountService.GetCustomerAccount(accountId);


            if (account.Balance < Amount)
            {
                ModelState.AddModelError("Amount", "Sorry, amount is greater than your current balance!");
            }

            if (accountTo == null)
            {
                ModelState.AddModelError("AccountToId", "Sorry, that account number does not exist!");
            }

            if (ModelState.IsValid)
            {
                _accountService.MakeTransfer(account, accountTo, Amount);

                // Tempdata is used together with toastr to display verification message
                TempData["success"] = "Transfer made successfully!";

                return RedirectToPage("/Customers/Customer", new { customerId = customerId });
            }

            // I have to fill "Transfer" even if model is invalid
            // If I don't I can't show Balance and account Id in HTML 
            Transfer = _accountService.GetNewTransferViewModel();
            Transfer.Account = _accountService.GetCustomerAccount(accountId);
            Transfer.Amount = 0;
            Transfer.CustomerId = customerId;

            return Page();
        }
    }
}
