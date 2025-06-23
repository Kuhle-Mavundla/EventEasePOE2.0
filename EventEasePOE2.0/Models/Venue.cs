using System.ComponentModel.DataAnnotations;

namespace EventEasePOE2._0.Models
{
    // This class holds information about a place where events can happen
    public class Venue
    {
        public int VenueId { get; set; }  // A special number to identify each venue

        [Required, StringLength(255)]
        public string Name { get; set; } = null!;  // The name of the venue (like "Big Hall")

        [Required, StringLength(255)]
        public string Location { get; set; } = null!;  // Where the venue is (like "Main Street")

        [Required]
        public int Capacity { get; set; }  // How many people can fit in the venue

        // The web address of the venue's picture stored in Azure Blob Storage
        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  // When the venue was added to the system

        // List of events happening at this venue
        public ICollection<Event> Events { get; set; } = new List<Event>();

        // List of bookings made for this venue
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
