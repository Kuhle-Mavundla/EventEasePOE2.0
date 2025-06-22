using System.ComponentModel.DataAnnotations;


namespace EventEasePOE2._0.Models
{
    public class Venue
    {
        public int VenueId { get; set; }

        [Required, StringLength(255)]
        public string Name { get; set; } = null!;

        [Required, StringLength(255)]
        public string Location { get; set; } = null!;

        [Required]
        public int Capacity { get; set; }

        // Stores blob URL but upload happens via Azure Blob service
        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Event> Events { get; set; } = new List<Event>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
