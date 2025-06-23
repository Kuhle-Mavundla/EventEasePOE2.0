using Microsoft.EntityFrameworkCore;
using EventEasePOE2._0.Models;

namespace EventEasePOE2._0.Data
{
    // This class helps talk to the database
    public class EventEasePOE2_0Context : DbContext
    {
        // This sets up the connection to the database using options
        public EventEasePOE2_0Context(DbContextOptions<EventEasePOE2_0Context> options) : base(options) { }

        // These lines tell the app about the tables in the database
        public DbSet<Venue> Venues { get; set; }      // Table for places where events happen
        public DbSet<Event> Events { get; set; }      // Table for the events themselves
        public DbSet<Booking> Bookings { get; set; }  // Table for reservations
        public DbSet<EventType> EventTypes { get; set; } // Table for types of events like concerts or weddings
    }
}
