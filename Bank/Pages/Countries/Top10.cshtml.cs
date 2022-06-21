using AutoMapper;
using Bank.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Bank.Pages.Countries
{
    [Authorize(Roles = "Cashier")]

    // Response caching example with parameters
    [ResponseCache(Duration = 60, VaryByQueryKeys = new[] { "countryId" })]

    public class Top10Model : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public Top10Model(ICustomerService customerService, IAccountService accountService, IMapper mapper)
        {
            _customerService = customerService;
            _accountService = accountService;
            _mapper = mapper;
        }

        public IEnumerable<CustomerCountryViewModel> Top10Customers { get; set; }
        public string Flag { get; set; }


        public class CustomerCountryViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; } = null!;
            public string? Emailaddress { get; set; }
            public decimal Balance { get; set; }

        }



        public void OnGet(string countryId)
        {
            Top10Customers = _customerService.GetCustomers()
            .AsQueryable()
            .Include(c => c.Dispositions)
            .ThenInclude(d => d.Account)
            //.Where(c => c.CountryCode == "DK")
            .Where(c => c.CountryCode == countryId)
            .SelectMany(c => c.Dispositions)
            .OrderByDescending(d => d.Account.Balance)
            .Take(10)
            .Select(d => new CustomerCountryViewModel
            {
                Id = d.CustomerId,
                Name = d.Customer.Surname + " " + d.Customer.Givenname,
                Emailaddress = d.Customer.Emailaddress,
                Balance = d.Account.Balance,
            })
            .ToList();

            // Set flag
            switch (countryId)
            {
                case "SE":
                    Flag = "/assets/img/flags/sweden-flag-png-large.png";
                    break;
                case "FI":
                    Flag = "/assets/img/flags/finland-flag-png-large.png";
                    break;
                case "DK":
                    Flag = "/assets/img/flags/denmark-flag-png-large.png";
                    break;
                case "NO":
                    Flag = "/assets/img/flags/norway-flag-png-large.png";
                    break;
            }

        }
    }
}
