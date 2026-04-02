using System;
using System.ComponentModel.DataAnnotations;

namespace kmc.API.Models
{
    public class CityActivity
    {
        [Key]
        public int ActivityId { get; set; }

        [Required]
        public string ActivityName { get; set; }

        [Required]
        public string Category { get; set; }

        public DateTime ScheduledDate { get; set; }

        public string Venue { get; set; }

        public int MaxParticipants { get; set; }

        [Required]
        public string OrganizerId { get; set; }
    }
}