using System;

namespace kmc.Client.Models
{
    public class CityActivityViewModel
    {
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public string Category { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string Venue { get; set; }
        public int MaxParticipants { get; set; }
        public string OrganizerId { get; set; }
    }
}