using kmc.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace kmc.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CityActivity> CityActivities { get; set; }
        public DbSet<EventBooking> EventBookings { get; set; }

        // 🌟 THE MISSING PIECE! This tells Identity to build its security tables correctly.
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // Do not remove this line!
        }
    }
}