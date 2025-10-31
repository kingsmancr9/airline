using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace airline1.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string From { get; set; } = string.Empty;

        [Required]
        public string To { get; set; } = string.Empty;

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int Children { get; set; }

        [Required]
        public string Currency { get; set; } = "USD";

        [Required]
        public string PassengerName { get; set; } = string.Empty;

        [Required]
        public string FlightNumber { get; set; } = string.Empty;

        [Required]
        public string DepartureTime { get; set; } = string.Empty;

        [Required]
        public string ArrivalTime { get; set; } = string.Empty;

        [Required]
        public string Duration { get; set; } = string.Empty;

        [Required]
        public decimal BaseFare { get; set; }

        [Required]
        public decimal Taxes { get; set; }

        [Required]
        public decimal ServiceCharge { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        [Required]
        public string Class { get; set; } = "Economy";

        [Required]
        public string Status { get; set; } = "Pending";

        [Required]
        public DateTime BookingDate { get; set; }

        public string? Email { get; set; }
        public string? Phone { get; set; }

        public ICollection<Payment>? Payments { get; set; }
    }
}
