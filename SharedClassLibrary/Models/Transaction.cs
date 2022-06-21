using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models
{
    public partial class Transaction
    {
        public int TransactionId { get; set; }
        public int AccountId { get; set; }
        public DateTime Date { get; set; }

        [Required]
        [MaxLength(50)]
        public string Type { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Operation { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(13, 2)")]
        public decimal Amount { get; set; }

        [Required]
        [Column(TypeName = "decimal(13, 2)")]
        public decimal Balance { get; set; }

        [MaxLength(50)]
        public string? Symbol { get; set; }

        [MaxLength(50)]
        public string? Bank { get; set; }

        [MaxLength(50)]
        public string? Account { get; set; }

        public virtual Account AccountNavigation { get; set; } = null!;

        public bool LaunderingChecked { get; set; } = false; // Couldnt get this to work!
    }
}
