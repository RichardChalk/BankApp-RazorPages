using AutoMapper;
using Bank.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Bank.Pages.Users
{
    [Authorize(Roles = "Admin")]
    [BindProperties]
    public class EditUserModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public EditUserViewModel EditUserView { get; set; } = new EditUserViewModel();


        public class EditUserViewModel
        {
            public string UserId { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "User role")]
            public string UserRole { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }


        public EditUserModel(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }


        public void OnGet(string userId)
        {
            
            var DbuserToEdit = _userService.GetUser(userId);


            EditUserView = new EditUserViewModel();

            // Automapper automatically maps all properties with the SAME NAME!
            // In this case we are mapping FROM db TO Frontend
            // _mapper.Map(customerToEdit, CustomerView);
            
            EditUserView.UserId = userId;
            EditUserView.Email = DbuserToEdit.Email;
            EditUserView.UserRole = _userService.GetRole(userId);
            //EditUserView.Password = DbuserToEdit.PasswordHash;
        }

        public async Task<IActionResult> OnPost(string userId)
        {
            EditUserView.UserId=userId;
            await _userService.EditUser(EditUserView);

            // Tempdata is used together with toastr to display verification message
            TempData["success"] = "User edited successfully!";

            return RedirectToPage("Users");
        }
    }
}
