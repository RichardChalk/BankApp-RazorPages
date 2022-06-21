using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bank.Models
{
    public class DataInitializer
    {
        private readonly JAFU20BankContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public DataInitializer(JAFU20BankContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        public void SeedData()
        {
            _dbContext.Database.Migrate();
            SeedRoles();
            SeedUsers();
            SeedFrequencies();
            SeedGenders();
            SeedCountries();
        }

        private void SeedFrequencies()
        {
            FrequencyDoesNotExist("AT", "After Transaction");
            FrequencyDoesNotExist("WE", "Weekly");
            FrequencyDoesNotExist("MO", "Monthly");
        }

        private void FrequencyDoesNotExist(string freqCode, string freqLabel)
        {
            // Check to see if frequncy already exists in db
            if (_dbContext.Frequencies.Any(f => f.FrequencyCode == freqCode)) return;
            _dbContext.Frequencies
                .Add(new Frequency
                {
                    FrequencyCode = freqCode,
                    FreqeuncyLabel = freqLabel
                });
            _dbContext.SaveChanges();
        }

        private void SeedGenders()
        {
            GenderDoesNotExist("MA", "Male");
            GenderDoesNotExist("FE", "Female");
        }

        private void GenderDoesNotExist(string genCode, string genLabel)
        {
            // Check to see if gender already exists in db
            if (_dbContext.Genders.Any(f => f.GenderCode == genCode)) return;
            _dbContext.Genders
                .Add(new Gender
                {
                    GenderCode = genCode,
                    GenderLabel = genLabel
                });
            _dbContext.SaveChanges();
        }

        private void SeedCountries()
        {
            CountryDoesNotExist("SE", "Sweden");
            CountryDoesNotExist("NO", "Norway");
            CountryDoesNotExist("DK", "Denmark");
            CountryDoesNotExist("FI", "Finland");
        }

        private void CountryDoesNotExist(string countryCode, string countryLabel)
        {
            // Check to see if country already exists in db
            if (_dbContext.Countries.Any(c => c.CountryCode == countryCode)) return;
            _dbContext.Countries
                .Add(new Country
                {
                    CountryCode = countryCode,
                    CountryLabel = countryLabel
                });
            _dbContext.SaveChanges();
        }

        private void SeedUsers()
        {
            AddUserIfNotExists("richard.chalk@admin.se", "Abc123#", new string[] { "Admin" });
            AddUserIfNotExists("richard.chalk@cashier.se", "Abc123#", new string[] { "Cashier" });

        }

        private void SeedRoles()
        {
            AddRoleIfNotExisting("Admin");
            AddRoleIfNotExisting("Cashier");
        }

        private void AddRoleIfNotExisting(string roleName)
        {
            var role = _dbContext.Roles.FirstOrDefault(r => r.Name == roleName);
            if (role == null)
            {
                _dbContext.Roles.Add(new IdentityRole { Name = roleName, NormalizedName = roleName });
                _dbContext.SaveChanges();
            }
        }


        private void AddUserIfNotExists(string userName, string password, string[] roles)
        {
            if (_userManager.FindByEmailAsync(userName).Result != null) return;

            var user = new IdentityUser
            {
                UserName = userName,
                Email = userName,
                EmailConfirmed = true
            };
            _userManager.CreateAsync(user, password).Wait();
            _userManager.AddToRolesAsync(user, roles).Wait();
        }
    }
}
