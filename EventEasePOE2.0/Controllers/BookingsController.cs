using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventEasePOE2._0.Services;
using EventEasePOE2._0.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using EventEasePOE2._0.Data;

namespace EventEasePOE2._0.Controllers
{
    public class BookingsController : Controller
    {
        private readonly EventEasePOE2_0Context _context;
        private readonly BookingService _bookingService;

        public BookingsController(EventEasePOE2_0Context context, BookingService bookingService)
        {
            _context = context;
            _bookingService = bookingService;
        }

        public async Task<IActionResult> Index(string eventName, int? venueId, string status, DateTime? startDate, DateTime? endDate)
        {
            ViewData["CurrentFilter"] = eventName;
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
            ViewBag.Venues = await _context.Venues.ToListAsync();

            var bookings = _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .AsQueryable();

            if (!string.IsNullOrEmpty(eventName))
                bookings = bookings.Where(b => b.Event != null && b.Event.Name.Contains(eventName));

            if (venueId.HasValue)
                bookings = bookings.Where(b => b.VenueId == venueId);

            if (!string.IsNullOrEmpty(status))
                bookings = bookings.Where(b => b.Status == status);

            if (startDate.HasValue)
                bookings = bookings.Where(b => b.StartDate >= startDate.Value);

            if (endDate.HasValue)
                bookings = bookings.Where(b => b.EndDate <= endDate.Value);

            return View(await bookings.ToListAsync());
        }
        public IActionResult Create()
        {
            ViewBag.Venues = new SelectList(_context.Venues, "VenueId", "Name");
            ViewBag.Events = new SelectList(_context.Events, "EventId", "Name");
            return View();
        }

        [HttpPost]
        public Task<IActionResult> Create(Booking booking)
        {
            if (ModelState.IsValid)
            {
                var success = _bookingService.TryCreateBooking(booking);
                if (success)
                    return Task.FromResult<IActionResult>(RedirectToAction(nameof(Index)));

                ModelState.AddModelError("", "This venue is not available during the selected period.");
            }
            ViewBag.Venues = new SelectList(_context.Venues, "VenueId", "Name", booking.VenueId);
            ViewBag.Events = new SelectList(_context.Events, "EventId", "Name", booking.EventId);
            return Task.FromResult<IActionResult>(View(booking));
        }

        public async Task<IActionResult> Details(int id)
        {
            var booking = await _context.Bookings.Include(b => b.Venue).Include(b => b.Event)
                .FirstOrDefaultAsync(b => b.BookingId == id);
            return booking == null ? NotFound() : View(booking);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            return booking == null ? NotFound() : View(booking);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
