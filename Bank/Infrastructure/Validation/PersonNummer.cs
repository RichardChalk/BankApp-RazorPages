using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Bank.Infrastructure.Validation
{
    public class PersonNummer : ValidationAttribute
    {
        private const string RegExForValidation = @"^\d{6}-\d{4}$";
        public override bool IsValid(object? value)
        {
            string input = value.ToString();

            return Regex.IsMatch(input, RegExForValidation);
        }
    }
}
