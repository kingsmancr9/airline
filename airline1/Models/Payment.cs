using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace airline1.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BookingId { get; set; }

        [ForeignKey("BookingId")]
        public Booking? Booking { get; set; }

        [Required]
        public string PaymentMethod { get; set; } = string.Empty;

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Currency { get; set; } = "USD";

        public string? TransactionId { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        [Required]
        public string Status { get; set; } = "Pending";

        // Optional payment method specific details
        public string? CardNumber { get; set; }
        public string? CardHolderName { get; set; }
        public string? ExpiryDate { get; set; }
        public string? UpiId { get; set; }
        public string? WalletType { get; set; }
    }
} 