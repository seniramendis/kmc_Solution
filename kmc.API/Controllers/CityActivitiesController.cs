using kmc.API.Data;
using kmc.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

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

        // GET: api/CityActivities (Open to everyone)
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

        // GET: api/CityActivities/5 (Open to everyone)
        [HttpGet("{id}")]
        public async Task<ActionResult<CityActivity>> GetCityActivity(int id)
        {
            var activity = await _context.CityActivities.FindAsync(id);
            if (activity == null) return NotFound();
            return activity;
        }

        // POST: api/CityActivities (LOCKED - Must be Organizer)
        [Authorize(Roles = "Organizer")]
        [HttpPost]
        public async Task<ActionResult<CityActivity>> PostCityActivity(CityActivity cityActivity)
        {
            _context.CityActivities.Add(cityActivity);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCityActivity), new { id = cityActivity.ActivityId }, cityActivity);
        }

        // PUT: api/CityActivities/5 (LOCKED - Must be Organizer)
        [Authorize(Roles = "Organizer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCityActivity(int id, CityActivity cityActivity)
        {
            if (id != cityActivity.ActivityId) return BadRequest("ID mismatch.");

            var existingActivity = await _context.CityActivities.AsNoTracking().FirstOrDefaultAsync(a => a.ActivityId == id);
            if (existingActivity == null) return NotFound("Activity not found.");

            if (existingActivity.OrganizerId != cityActivity.OrganizerId)
            {
                return Unauthorized("Only the creator can modify this activity.");
            }

            _context.Entry(cityActivity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/CityActivities/5 (LOCKED - Must be Organizer)
        [Authorize(Roles = "Organizer")]
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