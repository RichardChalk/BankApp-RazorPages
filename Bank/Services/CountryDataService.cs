using Microsoft.EntityFrameworkCore;

namespace Bank.Services
{
    public class CountryDataService : ICountryDataService
    {
        private readonly ICustomerService _customerService;
        public CountryDataService(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public int GetCountryCustomersCount(string country)
        {
            // Customers
            var customers = _customerService.GetCustomers()
                .Where(c => c.CountryCode == country);
            return customers.Count();
        }

        public int GetCountryAccountsCount(string country)
        {
            // Accounts
            var dispSE = _customerService.GetCustomers()
                .AsQueryable()
                .Include(c => c.Dispositions)
                .ThenInclude(d => d.Account)
                .Where(c => c.CountryCode == country)
                .SelectMany(c => c.Dispositions.Where(c => c.Type == "OWNER")); // Count only "Owners" of account (not joint accounts)
            return dispSE.Count();
        }

        public decimal GetCountryBalance(string country)
        {
            // Total Balance
            var dispSE = _customerService.GetCustomers()
                .AsQueryable()
                .Include(c => c.Dispositions)
                .ThenInclude(d => d.Account)
                .Where(c => c.CountryCode == country)
                .SelectMany(c => c.Dispositions);

            return dispSE
                .Select(d => d.Account.Balance)
                .Sum();
        }





    }
}
