using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models
{
    public partial class PermenentOrder
    {
        public int OrderId { get; set; }
        public int AccountId { get; set; }

        [Required]
        [MaxLength(50)]
        public string BankTo { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string AccountTo { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(13, 2)")]
        public decimal? Amount { get; set; }

        [Required]
        [MaxLength(50)]
        public string Symbol { get; set; } = null!;


        public virtual Account Account { get; set; } = null!;
    }
}
