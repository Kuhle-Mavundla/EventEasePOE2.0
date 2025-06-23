using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventEasePOE2._0.Services;
using EventEasePOE2._0.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using EventEasePOE2._0.Data;

namespace EventEasePOE2._0.Controllers
{
    // This is the controller that handles booking stuff
    public class BookingsController : Controller
    {
        // These are helpers to talk to the database and do booking things
        private readonly EventEasePOE2_0Context _context;
        private readonly BookingService _bookingService;

        // This is like a setup, it gives us tools to use later
        public BookingsController(EventEasePOE2_0Context context, BookingService bookingService)
        {
            _context = context; // database helper
            _bookingService = bookingService; // booking rules helper
        }

        // This shows a list of bookings, maybe filtered by some things
        public async Task<IActionResult> Index(string eventName, int? venueId, string status, DateTime? startDate, DateTime? endDate)
        {
            ViewData["CurrentFilter"] = eventName; // remember what event name was searched
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd"); // remember the start date filter
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd"); // remember the end date filter
            ViewBag.Venues = await _context.Venues.ToListAsync(); // get all venues to show in filter

            // Start with all bookings, including event and venue details
            var bookings = _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .AsQueryable();

            // If someone typed an event name, only show bookings for those events
            if (!string.IsNullOrEmpty(eventName))
                bookings = bookings.Where(b => b.Event != null && b.Event.Name.Contains(eventName));

            // If venue filter is set, only show bookings for that venue
            if (venueId.HasValue)
                bookings = bookings.Where(b => b.VenueId == venueId);

            // If status filter is set, only show bookings with that status
            if (!string.IsNullOrEmpty(status))
                bookings = bookings.Where(b => b.Status == status);

            // If a start date filter is set, only show bookings that start on or after that date
            if (startDate.HasValue)
                bookings = bookings.Where(b => b.StartDate >= startDate.Value);

            // If an end date filter is set, only show bookings that end on or before that date
            if (endDate.HasValue)
                bookings = bookings.Where(b => b.EndDate <= endDate.Value);

            // Show the filtered list of bookings on the screen
            return View(await bookings.ToListAsync());
        }

        // Show the page to create a new booking
        public IActionResult Create()
        {
            // Get lists of venues and events so we can choose them on the form
            ViewBag.Venues = new SelectList(_context.Venues, "VenueId", "Name");
            ViewBag.Events = new SelectList(_context.Events, "EventId", "Name");
            return View();
        }

        // This handles the form when someone clicks "Create" booking
        [HttpPost]
        public Task<IActionResult> Create(Booking booking)
        {
            // Check if everything filled in the form is okay
            if (ModelState.IsValid)
            {
                // Try to make the booking with our rules helper
                var success = _bookingService.TryCreateBooking(booking);

                // If it worked, go back to the list page
                if (success)
                    return Task.FromResult<IActionResult>(RedirectToAction(nameof(Index)));

                // If not, show an error message on the form
                ModelState.AddModelError("", "This venue is not available during the selected period.");
            }

            // If form is not valid or booking failed, reload venues and events lists so form can show them again
            ViewBag.Venues = new SelectList(_context.Venues, "VenueId", "Name", booking.VenueId);
            ViewBag.Events = new SelectList(_context.Events, "EventId", "Name", booking.EventId);

            // Show the form again with error messages
            return Task.FromResult<IActionResult>(View(booking));
        }

        // Show details of one booking when clicked
        public async Task<IActionResult> Details(int id)
        {
            // Find the booking with event and venue info
            var booking = await _context.Bookings.Include(b => b.Venue).Include(b => b.Event)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            // If not found, show "Not Found", else show the details page
            return booking == null ? NotFound() : View(booking);
        }

        // Show the page to confirm deleting a booking
        public async Task<IActionResult> Delete(int id)
        {
            // Find the booking to delete
            var booking = await _context.Bookings.FindAsync(id);

            // If not found, show "Not Found", else show the delete confirmation page
            return booking == null ? NotFound() : View(booking);
        }

        // Actually delete the booking after confirmation
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Find the booking to delete
            var booking = await _context.Bookings.FindAsync(id);

            // If no booking found, show "Not Found"
            if (booking == null)
            {
                return NotFound();
            }

            // Remove the booking from database
            _context.Bookings.Remove(booking);

            // Save the change
            await _context.SaveChangesAsync();

            // Go back to the bookings list
            return RedirectToAction(nameof(Index));
        }

    }
}
