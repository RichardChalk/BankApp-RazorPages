using Bank.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using SkysDemo3_1.Infrastructure.Paging;

namespace Bank.Services
{
    public interface ICustomerService
    {
        PagedResult<Customer> GetCustomers(int customerId, string sortColumn, string sortOrder, string searchText, int page);
        IEnumerable<Customer> GetCustomers();
        Customer GetCustomer(int customerId);
        JAFU20BankContext GetDbContext();
        List<SelectListItem> FillGenderDropDown();
        List<SelectListItem> FillCountryDropDown();
        Customer GetNewCustomer();
        void CreateCustomer(Customer customer);
        void EditCustomer();
        void Update();
    }


}
