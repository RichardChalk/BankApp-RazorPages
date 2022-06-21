using System.ComponentModel.DataAnnotations;

namespace Bank.Models
{
    public partial class User
    {
        public int UserId { get; set; }

        [Required]
        [MaxLength(40)]
        public string LoginName { get; set; } = null!;

        [Required]
        public byte[] PasswordHash { get; set; } = null!;


        [MaxLength(40)]
        public string? FirstName { get; set; }

        [MaxLength(40)]
        public string? LastName { get; set; }
    }
}
