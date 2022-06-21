using System.ComponentModel.DataAnnotations;

namespace Bank.Models
{

    public class Country
    {
        public int Id { get; set; }

        [MaxLength(2)]
        public string CountryCode { get; set; }

        [MaxLength(100)]
        public string CountryLabel { get; set; }

    }
}