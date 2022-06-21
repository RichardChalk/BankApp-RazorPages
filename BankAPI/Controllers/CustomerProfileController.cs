using AutoMapper;
using Bank.Models;
using Bank.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Controllers
{
    //[Route("api/[controller]")]
    [Produces("application/json")]
    [Route("api")]
    [ApiController]
    public class CustomerProfileController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;
        public CustomerProfileController(IAccountService accountService, ICustomerService customerService, IMapper mapper)
        {
            _accountService = accountService;
            _customerService = customerService;
            _mapper = mapper;
        }

        public CustomerProfileViewModel Customer { get; set; } = new CustomerProfileViewModel();


        public class CustomerProfileViewModel
        {
            public int Id { get; set; }
            public string Gender { get; set; } = null!;
            public string Givenname { get; set; } = null!;
            public string Surname { get; set; } = null!;
            public string Name { get; set; } = null!;
            public string Streetaddress { get; set; } = null!;
            public string City { get; set; } = null!;
            public string Zipcode { get; set; } = null!;
            public string Country { get; set; } = null!;
            public string CountryCode { get; set; } = null!;
            public DateTime Birthday { get; set; }
            public int Age { get; set; }
            public string? NationalId { get; set; }
            public string? Telephonecountrycode { get; set; }
            public string? Telephonenumber { get; set; }
            public string? Emailaddress { get; set; }
            public List<Account> Accounts { get; set; }
            public decimal TotalAccountValue { get; set; }
        }


        // READ ONE ///////////////////////////////////////////////////////
        /// <summary>
        /// Retrieve a SPECIFIC customer from the database
        /// </summary>
        /// <param name="id">
        /// Id of specific customer
        /// </param>
        /// <returns>
        /// The chosen customer (by Id)
        /// </returns>
        /// <remarks>
        /// Example end point: GET /api/me/1
        /// </remarks>
        /// <response code="200">
        /// Successfully returned the chosen customer (by Id)
        /// </response>
        [HttpGet("Me/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        //[Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetMe(int id)
        {

            var customerChosen = _customerService.GetCustomer(id);
            var accounts = _accountService.GetCustomerAccounts(id);

            if (customerChosen == null)
            {
                return NotFound("Customer not found");
            }
            // Data binding.... boring stuff
            Customer.Id = id;

            // Automapper automatically maps all properties with the SAME NAME!
            _mapper.Map(customerChosen, Customer);

            // Collect customer accounts
            Customer.Accounts = _accountService.GetCustomerAccounts(id).ToList();

            Customer.TotalAccountValue = Math.Round(Customer.Accounts
                .Select(a => a.Balance)
                .Sum(), 2);


            return Ok(Customer);
        }
    }
}
