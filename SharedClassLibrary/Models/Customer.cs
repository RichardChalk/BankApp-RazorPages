using System.ComponentModel.DataAnnotations;

namespace Bank.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Dispositions = new HashSet<Disposition>();
        }

        public int CustomerId { get; set; }

        [Required]
        public string Gender { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Givenname { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Surname { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Streetaddress { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string City { get; set; } = null!;

        [Required]
        [MaxLength(15)] // Matches current Db settings
        public string Zipcode { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Country { get; set; } = null!;

        [Required]
        [MaxLength(2)]
        public string CountryCode { get; set; } = null!;


        public DateTime Birthday { get; set; }


        [MaxLength(20)] // Matches current Db settings
        public string? NationalId { get; set; }

        [MaxLength(10)]
        public string? Telephonecountrycode { get; set; }

        [MaxLength(25)]
        public string? Telephonenumber { get; set; }

        [EmailAddress]
        public string? Emailaddress { get; set; }

        public virtual ICollection<Disposition> Dispositions { get; set; }
    }
}
