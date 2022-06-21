using AutoMapper;
using Bank.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Bank.Pages.Users
{
    [Authorize(Roles = "Admin")]
    [BindProperties]
    public class CreateUserModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public CreateUserViewModel UserView { get; set; } = new CreateUserViewModel();


        public class CreateUserViewModel
        {
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


        public CreateUserModel(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }


        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            await _userService.CreateUser(UserView);

            // Tempdata is used together with toastr to display verification message
            TempData["success"] = "User created successfully!";

            return RedirectToPage("Users");
        }
    }
}
