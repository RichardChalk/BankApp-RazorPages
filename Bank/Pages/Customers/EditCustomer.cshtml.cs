using AutoMapper;
using Bank.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Bank.Pages.Customers
{
    [Authorize(Roles = "Cashier")]
    [BindProperties]
    public class EditCustomerModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public EditCustomerViewModel CustomerView { get; set; } = new EditCustomerViewModel();

        public EditCustomerModel(IAccountService accountService, ICustomerService customerService, IMapper mapper)
        {
            _accountService = accountService;
            _customerService = customerService;
            _mapper = mapper;
        }

        public class EditCustomerViewModel
        {
            public int CustomerId { get; set; }

            [Required]
            public string Gender { get; set; } = null!;

            [Required]
            [MaxLength(100)]
            public string Givenname { get; set; } = null!;

            [Required]
            [MaxLength(100)]
            public string Surname { get; set; } = null!;

            [Required]
            [MaxLength(100)]
            public string Streetaddress { get; set; } = null!;

            [Required]
            [MaxLength(100)]
            public string City { get; set; } = null!;

            [Required]
            [MaxLength(15)] // Matches current Db settings
            public string Zipcode { get; set; } = null!;

            [Required]
            [MaxLength(100)]
            public string Country { get; set; } = null!;

            [DataType(DataType.Date)] // Removes time from datepicker
            public DateTime Birthday { get; set; }


            [MaxLength(20)] // Matches current Db settings
            public string? NationalId { get; set; }


            [MaxLength(25)]
            public string? Telephonenumber { get; set; }

            [EmailAddress]
            public string? Emailaddress { get; set; }

            // Used for dropdown
            public List<SelectListItem>? Genders { get; set; }
            public List<SelectListItem>? Countries { get; set; }

        }





        public void OnGet(int customerId)
        {
            var customerToEdit = _customerService.GetCustomer(customerId);


            CustomerView = new EditCustomerViewModel();

            // Automapper automatically maps all properties with the SAME NAME!
            // In this case we are mapping FROM db TO Frontend
            _mapper.Map(customerToEdit, CustomerView);

            // Populate Gender & Country DropDown list
            CustomerView.Genders = _customerService.FillGenderDropDown();
            CustomerView.Countries = _customerService.FillCountryDropDown();
        }

        public IActionResult OnPost(int customerId)
        {
            //var customer = _customerService.GetNewCustomer();
            CustomerView.CustomerId = customerId;
            var customer = _customerService.GetCustomer(customerId);

            // Automapper automatically maps all properties with the SAME NAME!
            _mapper.Map(CustomerView, customer);


            if (ModelState.IsValid)
            {
                _customerService.EditCustomer();

                // Tempdata is used together with toastr to display verification message
                TempData["success"] = "Customer edited successfully!";

                return RedirectToPage("/Customers/Customers");
            }

            return Page();
        }
    }
}
