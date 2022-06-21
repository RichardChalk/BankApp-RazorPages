using AutoMapper;
using Bank.Models;
using Bank.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bank.Pages.Customers
{
    [Authorize(Roles = "Cashier")]
    public class CustomerModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public CustomerModel(ICustomerService customerService, IAccountService accountService, IMapper mapper)
        {
            _customerService = customerService;
            _accountService = accountService;
            _mapper = mapper;
        }


        public CustomerProfileViewModel Customer { get; set; } = new CustomerProfileViewModel();



        // I havent written any data annotations here as this class is only reading from the db
        public class CustomerProfileViewModel
        {
            public int Id { get; set; }
            public string Gender { get; set; } = null!;
            public string Givenname { get; set; } = null!;
            public string Surname { get; set; } = null!;
            public string Name { get; set; } = null!;
            public string Streetaddress { get; set; } = null!;
            public string City { get; set; } = null!;
            public string Zipcode { get; set; } = null!;
            public string Country { get; set; } = null!;
            public string CountryCode { get; set; } = null!;
            public DateTime Birthday { get; set; }
            public int Age { get; set; }
            public string? NationalId { get; set; }
            public string? Telephonecountrycode { get; set; }
            public string? Telephonenumber { get; set; }
            public string? Emailaddress { get; set; }
            public List<Account> Accounts { get; set; }
            public decimal TotalAccountValue { get; set; }
        }


        public void OnGet(int customerId)
        {
            var customerChosen = _customerService.GetCustomer(customerId);
            var accounts = _accountService.GetCustomerAccounts(customerId);

            // Data binding.... boring stuff
            Customer.Id = customerId;

            // Automapper automatically maps all properties with the SAME NAME!
            // So we can delete all the below bindings!! (Commented out)
            _mapper.Map(customerChosen, Customer);

            //Customer.Gender = customerChosen.Gender;
            //Customer.Givenname = customerChosen.Givenname;
            //Customer.Surname = customerChosen.Surname;
            //Customer.Name = customerChosen.Givenname + " " + customerChosen.Surname;
            //Customer.Streetaddress = customerChosen.Streetaddress;
            //Customer.City = customerChosen.City;
            //Customer.Zipcode = customerChosen.Zipcode;
            //Customer.Country = customerChosen.Country;
            //Customer.CountryCode = customerChosen.CountryCode;
            //Customer.Birthday = customerChosen.Birthday;
            //Customer.Age = DateTime.Now.Year - customerChosen.Birthday.Year;
            //Customer.NationalId = customerChosen.NationalId;
            //Customer.Telephonecountrycode = customerChosen.Telephonecountrycode;
            //Customer.Telephonenumber = customerChosen.Telephonenumber;
            //Customer.Emailaddress = customerChosen.Emailaddress;

            // Collect customer accounts
            Customer.Accounts = _accountService.GetCustomerAccounts(customerId).ToList();

            Customer.TotalAccountValue = Math.Round(Customer.Accounts
                .Select(a => a.Balance)
                .Sum(), 2);
        }
    }
}
