using System.ComponentModel.DataAnnotations;

namespace EventEasePOE2._0.Models
{
    // This class stores information about the type of an event (like wedding or concert)
    public class EventType
    {
        public int EventTypeId { get; set; }  // A special number to identify each event type

        [Required, StringLength(100)]
        public string Name { get; set; } = null!;  // The name of the event type (like "Wedding")

        public string? Description { get; set; }  // Extra details about the event type (optional)

        public ICollection<Event> Events { get; set; } = new List<Event>();  // List of events that belong to this type
    }
}
