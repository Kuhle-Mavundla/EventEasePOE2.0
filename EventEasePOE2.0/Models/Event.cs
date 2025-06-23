using System.ComponentModel.DataAnnotations;

namespace EventEasePOE2._0.Models
{
    // This class stores information about an event (like a party or meeting)
    public class Event
    {
        public int EventId { get; set; }  // A special number to identify each event

        [Required, StringLength(255)]
        public string Name { get; set; } = null!;  // The name of the event (like "Birthday Party")

        public string? Description { get; set; }  // Extra details about the event (optional)

        [Required]
        public DateTime EventDate { get; set; }  // The date when the event will happen

        [Required]
        public int VenueId { get; set; }  // The ID of the place where the event will be held
        public Venue? Venue { get; set; }  // The actual place details (optional)

        [Required]
        public int EventTypeId { get; set; }  // The ID of the type of event (like wedding or concert)
        public EventType? EventType { get; set; }  // The actual event type details (optional)

        [Required, StringLength(50)]
        public string Status { get; set; } = "pending";  // The current status of the event (like pending or confirmed)

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  // When the event was created (right now)

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();  // All the bookings for this event (a list)
    }
}
