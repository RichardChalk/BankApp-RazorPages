using System.ComponentModel.DataAnnotations;

namespace Bank.Models
{

    public class Gender
    {
        public int Id { get; set; }

        [MaxLength(2)]
        public string GenderCode { get; set; }

        [MaxLength(100)]
        public string GenderLabel { get; set; }

    }
}