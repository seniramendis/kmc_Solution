using kmc.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace kmc.API.Data
{
    // We changed this from DbContext to IdentityDbContext!
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CityActivity> CityActivities { get; set; }
        public DbSet<EventBooking> EventBookings { get; set; }
    }
}