using EventEasePOE2._0.Data;
using EventEasePOE2._0.Models;

namespace EventEasePOE2._0.Services
{
    public class BookingService
    {
        private readonly EventEasePOE2_0Context _context;

        // This is a special box where we keep all our bookings (the database)
        public BookingService(EventEasePOE2_0Context context)
        {
            _context = context;
        }

        // This tries to make a new booking
        public bool TryCreateBooking(Booking newBooking)
        {
            // Check if the place is already booked when you want it
            var overlappingBooking = _context.Bookings
                .Where(b => b.VenueId == newBooking.VenueId && b.BookingId != newBooking.BookingId)
                .Any(b =>
                    // Does your start time come before someone else's end time
                    newBooking.StartDate < b.EndDate &&
                    // And does your end time come after someone else's start time?
                    newBooking.EndDate > b.StartDate);

            // If there is a booking that overlaps, we can't make your booking
            if (overlappingBooking)
            {
                return false; // Tell the app: "Sorry, it's not free"
            }

            // If no overlap, we add your booking to the box
            _context.Bookings.Add(newBooking);
            _context.SaveChanges(); // Save the booking for real

            return true; // Tell the app: "Booking is done!"
        }
    }
}
