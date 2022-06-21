using AutoMapper;
using Bank.Infrastructure.Validation;
using Bank.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Bank.Pages.Customers
{
    [Authorize(Roles = "Cashier")]
    [BindProperties]
    public class CreateCustomerModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public CreateCustomerViewModel CustomerView { get; set; } = new CreateCustomerViewModel();


        public class CreateCustomerViewModel
        {
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
            [PersonNummer(ErrorMessage = "Not a valid Swedish Social security number!")] // Own Attribute created
            public string? NationalId { get; set; }


            [MaxLength(25)]
            public string? Telephonenumber { get; set; }

            [EmailAddress]
            public string? Emailaddress { get; set; }

            // Used for dropdown
            public List<SelectListItem>? Genders { get; set; }
            public List<SelectListItem>? Countries { get; set; }

        }


        public CreateCustomerModel(IAccountService accountService, ICustomerService customerService, IMapper mapper)
        {
            _accountService = accountService;
            _customerService = customerService;
            _mapper = mapper;
        }


        public void OnGet()
        {
            CustomerView = new CreateCustomerViewModel();
            // Populate Gender & Country DropDown list
            CustomerView.Genders = _customerService.FillGenderDropDown();
            CustomerView.Countries = _customerService.FillCountryDropDown();

            // Setting the date to 1950 (otherwise it starts at year 0001)
            CustomerView.Birthday = new DateTime(1950, 01, 01);
        }

        public IActionResult OnPost()
        {
            // Populate Gender & Country DropDown list
            CustomerView.Genders = _customerService.FillGenderDropDown();
            CustomerView.Countries = _customerService.FillCountryDropDown();

            var customer = _customerService.GetNewCustomer();

            // Automapper automatically maps all properties with the SAME NAME!
            // In this case we are mapping FROM Frontend TO db
            _mapper.Map(CustomerView, customer);

            // Has to be a better way :(
            //customer.Gender = CustomerView.Gender;
            //customer.Givenname = CustomerView.Givenname;
            //customer.Surname = CustomerView.Surname;
            //customer.Streetaddress = CustomerView.Streetaddress;
            //customer.City = CustomerView.City;
            //customer.Zipcode = CustomerView.Zipcode;
            //customer.Country = CustomerView.Country;

            switch (CustomerView.Country)
            {
                case "Sweden":
                    customer.CountryCode = "SE";
                    customer.Telephonecountrycode = "46";
                    break;
                case "Norway":
                    customer.CountryCode = "NO";
                    customer.Telephonecountrycode = "47";
                    break;
                case "Denmark":
                    customer.CountryCode = "DK";
                    customer.Telephonecountrycode = "45";
                    break;
                case "Finland":
                    customer.CountryCode = "FI";
                    customer.Telephonecountrycode = "358";
                    break;

                default:
                    break;
            }

            //customer.Birthday = CustomerView.Birthday;
            //customer.NationalId = CustomerView.NationalId;
            //customer.Telephonenumber = CustomerView.Telephonenumber;
            //customer.Emailaddress = CustomerView.Emailaddress;


            if (ModelState.IsValid)
            {
                _customerService.CreateCustomer(customer);

                // Tempdata is used together with toastr to display verification message
                TempData["success"] = "Customer created successfully!";

                return RedirectToPage("/Customers/Customers");
            }

            return Page();
        }
    }
}
