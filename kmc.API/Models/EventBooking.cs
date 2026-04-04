using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        
        [ForeignKey("ActivityId")]
        public CityActivity? CityActivity { get; set; }
    }
}