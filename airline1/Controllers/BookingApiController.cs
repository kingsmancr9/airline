using airline1.Data;
using airline1.Models;
using Microsoft.AspNetCore.Mvc;

namespace airline1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingApiController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        public BookingApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Booking booking)
        {
            if (booking == null) return BadRequest();
            _context.Bookings.Add(booking);
            _context.SaveChanges();
            return Ok(new { message = "Booking saved!" });

        }
    }
}
