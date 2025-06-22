using System.ComponentModel.DataAnnotations;


namespace EventEasePOE2._0.Models
{
    public class EventType
    {
        public int EventTypeId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
