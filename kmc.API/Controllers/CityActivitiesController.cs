using kmc.API.Data;
using kmc.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace kmc.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityActivitiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CityActivitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CityActivities (Gets all events or searches by Category)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityActivity>>> GetCityActivities([FromQuery] string category = null)
        {
            if (!string.IsNullOrEmpty(category))
            {
                return await _context.CityActivities
                    .Where(a => a.Category.ToLower() == category.ToLower())
                    .ToListAsync();
            }
            return await _context.CityActivities.ToListAsync();
        }

        // GET: api/CityActivities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CityActivity>> GetCityActivity(int id)
        {
            var activity = await _context.CityActivities.FindAsync(id);
            if (activity == null) return NotFound();
            return activity;
        }

        // POST: api/CityActivities (Create a new activity)
        [HttpPost]
        public async Task<ActionResult<CityActivity>> PostCityActivity(CityActivity cityActivity)
        {
            _context.CityActivities.Add(cityActivity);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCityActivity), new { id = cityActivity.ActivityId }, cityActivity);
        }

        // PUT: api/CityActivities/5 (Update an activity)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCityActivity(int id, CityActivity cityActivity)
        {
            if (id != cityActivity.ActivityId) return BadRequest("ID mismatch.");

            var existingActivity = await _context.CityActivities.AsNoTracking().FirstOrDefaultAsync(a => a.ActivityId == id);
            if (existingActivity == null) return NotFound("Activity not found.");

            // Assignment Requirement: Only the Organizer can update their event
            if (existingActivity.OrganizerId != cityActivity.OrganizerId)
            {
                return Unauthorized("Only the creator can modify this activity.");
            }

            _context.Entry(cityActivity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/CityActivities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCityActivity(int id)
        {
            var cityActivity = await _context.CityActivities.FindAsync(id);
            if (cityActivity == null) return NotFound();

            _context.CityActivities.Remove(cityActivity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}