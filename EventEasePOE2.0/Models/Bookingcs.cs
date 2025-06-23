using System.ComponentModel.DataAnnotations;

namespace EventEasePOE2._0.Models
{
    // This class stores information about a booking (a reserved spot for an event)
    public class Booking
    {
        public int BookingId { get; set; }  // A unique number to identify each booking

        [Required]
        public int EventId { get; set; }    // The ID of the event being booked
        public Event? Event { get; set; }   // The actual event details (optional)

        [Required]
        public int VenueId { get; set; }    // The ID of the venue where the event will happen
        public Venue? Venue { get; set; }   // The actual venue details (optional)

        [Required]
        public DateTime StartDate { get; set; }  // When the booking starts (the date and time)

        [Required]
        public DateTime EndDate { get; set; }    // When the booking ends (the date and time)

        [Required, StringLength(50)]
        public string Status { get; set; } = "pending"; // Status of the booking (like pending or confirmed)

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // When the booking was made (right now)
    }
}
