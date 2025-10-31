using airline1.Data;
using airline1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace airline1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BookingController> _logger;

        public BookingController(ApplicationDbContext context, ILogger<BookingController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("CreateBooking")]
        public async Task<IActionResult> CreateBooking([FromBody] Booking booking)
        {
            try
            {
                _logger.LogInformation("Received booking request: {BookingData}", JsonSerializer.Serialize(booking));

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage);
                    
                    _logger.LogError("Invalid booking model state. Errors: {Errors}", string.Join(", ", errors));
                    return BadRequest(new { errors });
                }

                // Validate required fields
                if (string.IsNullOrEmpty(booking.From) || string.IsNullOrEmpty(booking.To))
                {
                    _logger.LogError("Missing required fields: From or To");
                    return BadRequest(new { error = "From and To locations are required" });
                }

                if (booking.Date == default)
                {
                    _logger.LogError("Invalid date");
                    return BadRequest(new { error = "Valid date is required" });
                }

                booking.BookingDate = DateTime.UtcNow;
                booking.Status = "Pending";

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Booking created successfully with ID: {BookingId}", booking.Id);
                return Ok(new { bookingId = booking.Id });
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while creating booking");
                return StatusCode(500, new { error = "Database error occurred while creating the booking" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating booking");
                return StatusCode(500, new { error = "An error occurred while creating the booking" });
            }
        }

        [HttpPost("ProcessPayment")]
        public async Task<IActionResult> ProcessPayment([FromBody] Payment payment)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid payment model state: {ModelState}", ModelState);
                    return BadRequest(ModelState);
                }

                // Verify booking exists
                var booking = await _context.Bookings.FindAsync(payment.BookingId);
                if (booking == null)
                {
                    _logger.LogError("Booking not found for ID: {BookingId}", payment.BookingId);
                    return NotFound("Booking not found");
                }

                payment.PaymentDate = DateTime.UtcNow;
                payment.Status = "Completed";
                payment.TransactionId = Guid.NewGuid().ToString();

                _context.Payments.Add(payment);

                // Update booking status
                booking.Status = "Completed";
                await _context.SaveChangesAsync();

                _logger.LogInformation("Payment processed successfully for booking ID: {BookingId}", payment.BookingId);
                return Ok(new { paymentId = payment.Id, transactionId = payment.TransactionId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment");
                return StatusCode(500, "An error occurred while processing the payment");
            }
        }

        [HttpGet("GetBookingDetails/{id}")]
        public async Task<IActionResult> GetBookingDetails(int id)
        {
            try
            {
                var booking = await _context.Bookings
                    .Include(b => b.Payments)
                    .FirstOrDefaultAsync(b => b.Id == id);

                if (booking == null)
                {
                    _logger.LogWarning("Booking not found for ID: {BookingId}", id);
                    return NotFound();
                }

                return Ok(booking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving booking details for ID: {BookingId}", id);
                return StatusCode(500, "An error occurred while retrieving booking details");
            }
        }

        [HttpGet("GetAllBookings")]
        public async Task<IActionResult> GetAllBookings()
        {
            try
            {
                var bookings = await _context.Bookings
                    .Include(b => b.Payments)
                    .OrderByDescending(b => b.BookingDate)
                    .ToListAsync();

                return Ok(bookings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all bookings");
                return StatusCode(500, "An error occurred while retrieving bookings");
            }
        }
    }
} 