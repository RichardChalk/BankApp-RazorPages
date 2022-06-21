using AutoMapper;
using Bank.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Bank.Pages.Users
{
    [Authorize(Roles = "Admin")]
    public class UsersModel : PageModel
    {

        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public List<UserViewModel> Users { get; set; }

        public int Id { get; set; }
        public string SortOrder { get; set; }
        public string SortColumn { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public string SearchText { get; set; }



        public UsersModel(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }


        public class UserViewModel
        {
            public string Id { get; set; }

            [MaxLength(12)]
            public string UserName { get; set; } = null!;

            [MaxLength(70)]
            public string Email { get; set; } = null!;

            [MaxLength(70)]
            public string Role { get; set; } = null!;

        }

        public void OnGet(int userId, string sortColumn, string sortOrder, string searchText, int pageNo)
        {
            SearchText = searchText;
            SortOrder = sortOrder;
            SortColumn = sortColumn;
            if (pageNo == 0)
                pageNo = 1;
            CurrentPage = pageNo;
            Id = userId;

            var pageresult = _userService.GetUsers(userId, sortColumn, sortOrder, searchText, CurrentPage);
            PageCount = pageresult.PageCount;

            Users = pageresult.Results
                .Select(u => new UserViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    Role = _userService.GetRole(u.Id),
                })
                .ToList();

            // Users = _mapper.Map<List<UserViewModel>>(pageresult.Results);
        }
    }
}
