using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models
{
    public partial class Loan
    {
        public int LoanId { get; set; }
        public int AccountId { get; set; }
        public DateTime Date { get; set; }

        [Required]
        [Column(TypeName = "decimal(13, 2)")]
        public decimal Amount { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        [Column(TypeName = "decimal(13, 2)")]
        public decimal Payments { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = null!;

        public virtual Account Account { get; set; } = null!;
    }
}
