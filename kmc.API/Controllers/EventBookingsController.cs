using kmc.API.Data;
using kmc.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims; 

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

            
            var userEmail = User.FindFirstValue(ClaimTypes.Name) ?? User.Identity?.Name;
            if (string.IsNullOrEmpty(userEmail)) return BadRequest("Security Error: Could not read your email from the token.");

            booking.ContactEmail = userEmail;

            _context.EventBookings.Add(booking);
            await _context.SaveChangesAsync();

            return Ok(booking);
        }

        
        [Authorize(Roles = "Resident")]
        [HttpGet("MyBookings")]
        public async Task<ActionResult<IEnumerable<EventBooking>>> GetMyBookings()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Name) ?? User.Identity?.Name;

            var myBookings = await _context.EventBookings
                .Include(b => b.CityActivity)
                .Where(b => b.ContactEmail == userEmail)
                .ToListAsync();

            return myBookings;
        }

        
        [Authorize(Roles = "Organizer")]
        [HttpGet("Activity/{activityId}")]
        public async Task<ActionResult<IEnumerable<EventBooking>>> GetBookingsForActivity(int activityId)
        {
            var bookings = await _context.EventBookings
                .Where(b => b.ActivityId == activityId)
                .ToListAsync();

            return bookings;
        }

        
        [Authorize(Roles = "Resident")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.EventBookings.FindAsync(id);
            if (booking == null) return NotFound("Booking not found.");

            
            var userEmail = User.FindFirstValue(ClaimTypes.Name) ?? User.Identity?.Name;
            if (booking.ContactEmail != userEmail)
            {
                return Forbid("You can only cancel your own tickets.");
            }

            _context.EventBookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}