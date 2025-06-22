using Microsoft.EntityFrameworkCore;
using EventEasePOE2._0.Models;

namespace EventEasePOE2._0.Data
{
    public class EventEasePOE2_0Context : DbContext
    {
        public EventEasePOE2_0Context(DbContextOptions<EventEasePOE2_0Context> options) : base(options) { }

        public DbSet<Venue> Venues { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
    }
}
