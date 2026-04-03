using kmc.API.Data;
using kmc.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace kmc.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventBookingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EventBookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/EventBookings (LOCKED - Resident Only)
        [Authorize(Roles = "Resident")]
        [HttpPost]
        public async Task<ActionResult<EventBooking>> PostEventBooking(EventBooking booking)
        {
            var activity = await _context.CityActivities.FindAsync(booking.ActivityId);
            if (activity == null) return NotFound("Activity not found.");

            var currentBookingsCount = await _context.EventBookings.CountAsync(b => b.ActivityId == booking.ActivityId);
            if (currentBookingsCount >= activity.MaxParticipants)
            {
                return BadRequest("Sorry, this activity is already full!");
            }

            _context.EventBookings.Add(booking);
            await _context.SaveChangesAsync();

            return Ok(booking);
        }

        // GET: api/EventBookings/MyBookings (LOCKED - Resident Only)
        [Authorize(Roles = "Resident")]
        [HttpGet("MyBookings")]
        public async Task<ActionResult<IEnumerable<EventBooking>>> GetMyBookings()
        {
            var userEmail = User.Identity.Name;

            var myBookings = await _context.EventBookings
                .Include(b => b.CityActivity)
                .Where(b => b.ContactEmail == userEmail)
                .ToListAsync();

            return myBookings;
        }

        // 🌟 NEW: GET: api/EventBookings/Activity/5 (LOCKED - Organizer Only)
        [Authorize(Roles = "Organizer")]
        [HttpGet("Activity/{activityId}")]
        public async Task<ActionResult<IEnumerable<EventBooking>>> GetBookingsForActivity(int activityId)
        {
            var bookings = await _context.EventBookings
                .Where(b => b.ActivityId == activityId)
                .ToListAsync();

            return bookings;
        }
    }
}