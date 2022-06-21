using Bank.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SkysDemo3_1.Infrastructure.Paging;

namespace Bank.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly JAFU20BankContext _context;
        private readonly IAccountService _accountService;

        public CustomerService(JAFU20BankContext context, IAccountService accountService)
        {
            _context = context;
            _accountService = accountService;
        }
        public PagedResult<Customer> GetCustomers(int customerId, string sortColumn, string sortOrder, string searchText, int page)
        {
            var query = _context.Customers.AsQueryable();

            if (sortColumn == "CustomerId" || string.IsNullOrEmpty(sortColumn))
                if (sortOrder == "asc")
                    query = query.OrderBy(c => c.CustomerId);
                else
                    query = query.OrderByDescending(c => c.CustomerId);

            if (sortColumn == "NationalId" || string.IsNullOrEmpty(sortColumn))
                if (sortOrder == "asc")
                    query = query.OrderBy(c => c.NationalId);
                else
                    query = query.OrderByDescending(c => c.NationalId);

            else if (sortColumn == "FullName" || string.IsNullOrEmpty(sortColumn))
                if (sortOrder == "desc")
                    query = query.OrderByDescending(c => c.Surname).ThenByDescending(c => c.Givenname);
                else
                    query = query.OrderBy(c => c.Surname).ThenByDescending(c => c.Givenname);


            else if (sortColumn == "Address" || string.IsNullOrEmpty(sortColumn))
                if (sortOrder == "asc")
                    query = query.OrderBy(c => c.Streetaddress);
                else
                    query = query.OrderByDescending(c => c.Streetaddress);

            else if (sortColumn == "City" || string.IsNullOrEmpty(sortColumn))
                if (sortOrder == "asc")
                    query = query.OrderBy(c => c.City);
                else
                    query = query.OrderByDescending(c => c.City);

            //Om det finns någon söksträng...
            if (searchText != null)
            {
                query = query
                .Where(c =>
                c.CustomerId.ToString() == searchText ||
                c.Givenname.ToLower().Contains(searchText.ToLower()) ||
                c.Surname.ToLower().Contains(searchText.ToLower()) ||
                c.City.ToLower().Contains(searchText.ToLower())
                );
            }

            return query.GetPaged(page, 50);
        }

        // Get customers for start page
        public IEnumerable<Customer> GetCustomers()
        {
            var customers = _context.Customers;
            return customers;
        }



        // Get a single customer for Customer Profile page
        public Customer GetCustomer(int customerId)
        {
            var customer = _context.Customers
                .Include(c => c.Dispositions)
                .First(c => c.CustomerId == customerId);

            return customer;
        }

        // Get a DbContext
        public JAFU20BankContext GetDbContext()
        {
            var context = _context;

            return context;
        }

        // Save to db
        public void Update()
        {
            _context.SaveChanges();
        }

        // Populate gender list
        public List<SelectListItem> FillGenderDropDown()
        {
            List<SelectListItem> genders = new List<SelectListItem>();
            genders = _context.Genders.Select(g => new SelectListItem
            {
                Text = g.GenderLabel,
                Value = g.GenderLabel,
            })
                .ToList();

            genders.Insert(0, new SelectListItem
            {
                Text = "Choose one...",
                Value = ""
            });

            return genders;
        }

        // Populate country list
        public List<SelectListItem> FillCountryDropDown()
        {
            List<SelectListItem> countries = new List<SelectListItem>();
            countries = _context.Countries.Select(c => new SelectListItem
            {
                Text = c.CountryLabel,
                Value = c.CountryLabel,
            })
                .ToList();

            countries.Insert(0, new SelectListItem
            {
                Text = "Choose one...",
                Value = ""
            });

            return countries;
        }


        // Create a fresh customer
        public Customer GetNewCustomer()
        {
            var customer = new Customer();

            return customer;
        }

        public void CreateCustomer(Customer customer)
        {
            var account = _accountService.GetNewAccount();
            var disposition = _accountService.GetNewDisposition();

            // Tie diposition to correct customer
            disposition.Account = account;
            disposition.Type = "OWNER";
            account.Frequency = "Monthly"; // Create a monthly frequency as standard
            // Set todays date as account creation
            DateTime today = DateTime.Now;
            account.Created = today;

            // Save customer and lookup table to Db
            customer.Dispositions.Add(disposition);
            _context.Customers.Add(customer);

            _context.SaveChanges();
        }

        public void EditCustomer()
        {
            // Feels kinda unnecessary.... I could have simply saved changes in the editCustomer page...
            _context.SaveChanges();
        }
    }
}
