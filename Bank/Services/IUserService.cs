using Microsoft.AspNetCore.Identity;
using SkysDemo3_1.Infrastructure.Paging;
using static Bank.Pages.Users.CreateUserModel;
using static Bank.Pages.Users.EditUserModel;

namespace Bank.Services
{
    public interface IUserService
    {
        PagedResult<IdentityUser> GetUsers(int userId, string sortColumn, string sortOrder, string searchText, int page);
        string GetRole(string userId);
        Task CreateUser(CreateUserViewModel newUser);
        Task EditUser(EditUserViewModel editUser);
        IdentityUser GetUser(string userId);
    }
}
