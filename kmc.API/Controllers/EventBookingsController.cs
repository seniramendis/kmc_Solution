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

        // POST: api/EventBookings (LOCKED - Must be a Resident)
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
        // 🌟 NEW: This method grabs the user's email from their Token and finds their tickets!
        [Authorize(Roles = "Resident")]
        [HttpGet("MyBookings")]
        public async Task<ActionResult<IEnumerable<EventBooking>>> GetMyBookings()
        {
            // Extract the logged-in user's email from their Token
            var userEmail = User.Identity.Name;

            var myBookings = await _context.EventBookings
                .Include(b => b.CityActivity) // Automatically joins the Event data so we know the name of the activity!
                .Where(b => b.ContactEmail == userEmail)
                .ToListAsync();

            return myBookings;
        }
    }
}