using System.ComponentModel.DataAnnotations;

namespace Bank.Models
{

    public class Frequency
    {
        public int Id { get; set; }

        [MaxLength(2)]
        public string FrequencyCode { get; set; }

        [MaxLength(100)]
        public string FreqeuncyLabel { get; set; }

    }
}