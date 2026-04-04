using System;
using System.ComponentModel.DataAnnotations;

namespace kmc.Client.Models
{
    public class EventBookingViewModel
    {
        public int BookingId { get; set; }

        [Required]
        public int ActivityId { get; set; }

        [Required(ErrorMessage = "Please enter your full name.")]
        public string ResidentName { get; set; }

        [Required(ErrorMessage = "Please enter your email.")]
        [EmailAddress]
        public string ContactEmail { get; set; }

        public DateTime BookingDate { get; set; } = DateTime.UtcNow;

        
        public CityActivityViewModel? CityActivity { get; set; }
    }
}