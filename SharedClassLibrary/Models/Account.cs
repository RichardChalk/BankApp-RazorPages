using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Models
{
    public partial class Account
    {
        public Account()
        {
            Dispositions = new HashSet<Disposition>();
            Loans = new HashSet<Loan>();
            PermenentOrders = new HashSet<PermenentOrder>();
            Transactions = new HashSet<Transaction>();
        }



        [Range(1, 99999)]
        public int AccountId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Frequency { get; set; } = null!;

        [Required]
        public DateTime Created { get; set; }

        [Required]
        [Column(TypeName = "decimal(13, 2)")]
        public decimal Balance { get; set; }

        public virtual ICollection<Disposition> Dispositions { get; set; }
        public virtual ICollection<Loan> Loans { get; set; }
        public virtual ICollection<PermenentOrder> PermenentOrders { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
