using System.ComponentModel.DataAnnotations;

namespace Bank.Models
{
    public partial class Card
    {
        public int CardId { get; set; }

        [Required]
        public int DispositionId { get; set; }

        [Required]
        [RegularExpression("junior|classic|gold", ErrorMessage = "Not a valid card type (Classic, Gold, Junior)")]
        public string Type { get; set; } = null!;

        [Required]
        public DateTime Issued { get; set; }

        [Required]
        [RegularExpression("MasterCard|Visa", ErrorMessage = "Not a valid card type (MasterCard, Visa)")]
        public string Cctype { get; set; } = null!;

        [Required]
        [MinLength(16)]
        [MaxLength(16)]
        public string Ccnumber { get; set; } = null!;

        [Required]
        [MinLength(3)]
        [MaxLength(3)]
        public string Cvv2 { get; set; } = null!;

        [Required]
        [Range(1, 12)]
        public int ExpM { get; set; }
        [Required]
        [Range(2020, 9999)]
        public int ExpY { get; set; }

        public virtual Disposition Disposition { get; set; } = null!;
    }
}
