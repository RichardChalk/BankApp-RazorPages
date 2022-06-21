using Microsoft.AspNetCore.Identity;
using SkysDemo3_1.Infrastructure.Paging;
using static Bank.Pages.Users.CreateUserModel;
using static Bank.Pages.Users.EditUserModel;

namespace Bank.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public PagedResult<IdentityUser> GetUsers(int userId, string sortColumn, string sortOrder, string searchText, int page)
        {
            var query = _userManager.Users.AsQueryable();

            if (sortColumn == "UserId" || string.IsNullOrEmpty(sortColumn))
                if (sortOrder == "asc")
                    query = query.OrderBy(u => u.Id);
                else
                    query = query.OrderByDescending(u => u.Id);

            if (sortColumn == "UserName" || string.IsNullOrEmpty(sortColumn))
                if (sortOrder == "asc")
                    query = query.OrderBy(u => u.UserName);
                else
                    query = query.OrderByDescending(u => u.UserName);

            if (sortColumn == "Email" || string.IsNullOrEmpty(sortColumn))
                if (sortOrder == "desc")
                    query = query.OrderByDescending(u => u.Email);
                else
                    query = query.OrderBy(u => u.Email);



            //Om det finns någon söksträng...
            if (searchText != null)
            {
                query = query
                .Where(u =>
                u.Id.ToString() == searchText ||
                u.UserName.ToLower().Contains(searchText.ToLower()) ||
                u.Email.ToLower().Contains(searchText.ToLower())
                );
            }


            return query.GetPaged(page, 50);
        }

        public string GetRole(string userId)
        {
            var user = _userManager.Users
                .Where(u => u.Id == userId)
                .First();

            var role = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().First();

            return role;
        }

        public async Task CreateUser(CreateUserViewModel userViewModel)
        {

            var user = new IdentityUser();
            user.Email = userViewModel.Email;
            user.UserName = userViewModel.Email.ToLower();
            user.EmailConfirmed = true;

            var result = await _userManager.CreateAsync(user, userViewModel.Password);

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            await _userManager.AddToRoleAsync(user, userViewModel.UserRole);
        }

        public async Task EditUser(EditUserViewModel userViewModel)
        {

            var Dbuser = _userManager.Users
                .First(u => u.Id == userViewModel.UserId);

            var code = await _userManager.GeneratePasswordResetTokenAsync(Dbuser);

            var result = await _userManager.ResetPasswordAsync(Dbuser, code, userViewModel.Password);
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            await _userManager.RemoveFromRoleAsync(Dbuser, "Admin");
            await _userManager.RemoveFromRoleAsync(Dbuser, "Cashier");

            await _userManager.AddToRoleAsync(Dbuser, userViewModel.UserRole);

            var roles = await _userManager.GetRolesAsync(Dbuser);
        }

        public IdentityUser GetUser(string userId)
        {
            var user = _userManager.Users
                .First(u => u.Id == userId);

            return user;
        }

    }
}
