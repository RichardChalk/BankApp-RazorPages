using AutoMapper;
using Bank.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Bank.Pages.Customers
{
    [Authorize(Roles = "Cashier")]
    public class CustomersModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;
        public List<CustomerViewModel> Customers { get; set; }

        public int Id { get; set; }
        public string SortOrder { get; set; }
        public string SortColumn { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public string SearchText { get; set; }



        public CustomersModel(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }


        public class CustomerViewModel
        {
            public int Id { get; set; }
            [MaxLength(12)]
            public string NationalId { get; set; } = null!;
            [MaxLength(70)]
            public string Givenname { get; set; } = null!;
            [MaxLength(70)]
            public string Surname { get; set; } = null!;
            [MaxLength(140)]
            public string FullName { get; set; } = null!;
            [MaxLength(100)]
            public string StreetAddress { get; set; } = null!;
            [MaxLength(100)]
            public string City { get; set; } = null!;

        }

        public void OnGet(int customerId, string sortColumn, string sortOrder, string searchText, int pageNo)
        {
            SearchText = searchText;
            SortOrder = sortOrder;
            SortColumn = sortColumn;
            if (pageNo == 0)
                pageNo = 1;
            CurrentPage = pageNo;
            Id = customerId;

            var pageresult = _customerService.GetCustomers(customerId, sortColumn, sortOrder, searchText, CurrentPage);
            PageCount = pageresult.PageCount;

            // We can use AutoMapper so we dont need all these mappings anymore!
            //Customers = pageresult.Results
            //    .Select(c => new CustomerViewModel
            //    {
            //        Id = c.CustomerId,
            //        NationalId = c.NationalId,
            //        FullName = c.Surname + ", " + c.Givenname,
            //        StreetAddress = c.Streetaddress,
            //        City = c.City,

            //    })
            //    .ToList();

            Customers = _mapper.Map<List<CustomerViewModel>>(pageresult.Results);
        }
    }
}
