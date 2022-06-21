using AutoMapper;
using Bank.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Bank.Pages.Accounts
{
    [Authorize(Roles = "Cashier")]
    public class AccountsModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        public List<AccountViewModel> Accounts { get; set; }

        public int Id { get; set; }
        public string SortOrder { get; set; }
        public string SortColumn { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public string SearchText { get; set; }



        public AccountsModel(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }


        public class AccountViewModel
        {
            public int Id { get; set; }

            [MaxLength(70)]
            public string Frequency { get; set; } = null!;

            public DateTime Created { get; set; }

            [MaxLength(70)]
            public decimal Balance { get; set; }

        }



        public void OnGet(string sortColumn, string sortOrder, string searchText, int pageNo)
        {
            SearchText = searchText;
            SortOrder = sortOrder;
            SortColumn = sortColumn;
            if (pageNo == 0)
                pageNo = 1;
            CurrentPage = pageNo;

            var pageresult = _accountService.GetAccounts(sortColumn, sortOrder, searchText, CurrentPage);
            PageCount = pageresult.PageCount;

            // We can use AUTOMAPPER to return a list of "AccountViewModels"
            //Accounts = pageresult.Results
            //    .Select(a => new AccountViewModel
            //    {
            //        Id = a.AccountId,
            //        Frequency = a.Frequency,
            //        Created = a.Created,
            //        Balance = a.Balance,
            //    })
            //    .ToList();

            Accounts = _mapper.Map<List<AccountViewModel>>(pageresult.Results);

        }
    }
}
