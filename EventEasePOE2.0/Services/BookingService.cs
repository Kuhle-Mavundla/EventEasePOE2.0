using EventEasePOE2._0.Data;
using EventEasePOE2._0.Models;

namespace EventEasePOE2._0.Services
{
    public class BookingService
    {
        private readonly EventEasePOE2_0Context _context;

        public BookingService(EventEasePOE2_0Context context)
        {
            _context = context;
        }

        public bool TryCreateBooking(Booking newBooking)
        {
            // Check for overlapping bookings for the same venue
            var overlappingBooking = _context.Bookings
                .Where(b => b.VenueId == newBooking.VenueId && b.BookingId != newBooking.BookingId)
                .Any(b =>
                    newBooking.StartDate < b.EndDate &&
                    newBooking.EndDate > b.StartDate);

            if (overlappingBooking)
            {
                return false;
            }

            _context.Bookings.Add(newBooking);
            _context.SaveChanges();
            return true;
        }
    }
}