using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // 🌟 ADDED: We need this to use the ForeignKey tag!

namespace kmc.API.Models
{
    public class EventBooking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        public int ActivityId { get; set; }

        [Required]
        public string ResidentName { get; set; }

        [Required]
        public string ContactEmail { get; set; }

        public DateTime BookingDate { get; set; } = DateTime.UtcNow;

        // 🌟 ADDED: This explicitly tells the database to use ActivityId to join the tables!
        [ForeignKey("ActivityId")]
        public CityActivity CityActivity { get; set; }
    }
}