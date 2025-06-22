using System.ComponentModel.DataAnnotations;

namespace EventEasePOE2._0.Models
{
    public class Event
    {
        public int EventId { get; set; }

        [Required, StringLength(255)]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        public int VenueId { get; set; }
        public Venue? Venue { get; set; }

        [Required]
        public int EventTypeId { get; set; }
        public EventType? EventType { get; set; }

        [Required, StringLength(50)]
        public string Status { get; set; } = "pending";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
