using Bank.Models;
using Bank.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Pages.Accounts
{
    [Authorize(Roles = "Cashier")]

    [BindProperties]
    public class CreateModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly ICustomerService _customerService;


        // Properties for Account
        public int AccountId { get; set; }

        [Required(ErrorMessage = "Please choose a valid frequency!")]
        public string Frequency { get; set; }

        public DateTime Created { get; set; }

        [Required]
        [Column(TypeName = "decimal(13, 2)")]
        [Range(0, int.MaxValue)]
        public decimal Balance { get; set; }

        public ICollection<Disposition> Dispositions { get; set; }

        // Used for dropdown
        public List<SelectListItem> Frequencies { get; set; }



        // I dont fill these-----
        public ICollection<Loan> Loans { get; set; }
        public ICollection<PermenentOrder> PermenentOrders { get; set; }
        public ICollection<Transaction> Transactions { get; set; }


        public int CustomerId { get; set; }


        public CreateModel(IAccountService accountService, ICustomerService customerService)
        {
            _accountService = accountService;
            _customerService = customerService;
        }


        public void OnGet(int customerId)
        {
            // "These are not the droids you are looking for...."
            CustomerId = customerId;

            // Populate Frequency DropDown list
            Frequencies = _accountService.FillFrequencyDropDown();
        }

        public IActionResult OnPost(int customerId)
        {
            if (ModelState.IsValid)
            {
                _accountService.CreateAccount(customerId, Frequency, Balance);

                // This is needed in order to route back to the same customers page
                var customer = _customerService.GetCustomer(customerId);

                // Tempdata is used together with toastr to display verification message
                TempData["success"] = "Account created successfully!";

                return RedirectToPage("/Customers/Customer", new { customerId = customer.CustomerId });
            }

            return Page();
        }
    }
}
