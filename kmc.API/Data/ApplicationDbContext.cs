using kmc.API.Models;
using Microsoft.EntityFrameworkCore;

namespace kmc.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CityActivity> CityActivities { get; set; }
        public DbSet<EventBooking> EventBookings { get; set; }
    }
}