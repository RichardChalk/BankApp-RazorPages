using System.ComponentModel.DataAnnotations;

namespace Bank.Models
{
    public partial class Disposition
    {
        public Disposition()
        {
            Cards = new HashSet<Card>();
        }

        public int DispositionId { get; set; }
        public int CustomerId { get; set; }

        public int AccountId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Type { get; set; } = null!;

        public virtual Account Account { get; set; } = null!;
        public virtual Customer Customer { get; set; } = null!;
        public virtual ICollection<Card> Cards { get; set; }
    }
}
