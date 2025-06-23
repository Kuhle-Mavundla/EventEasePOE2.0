using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventEasePOE2._0.Services;
using EventEasePOE2._0.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using EventEasePOE2._0.Data;

namespace EventEasePOE2._0.Controllers
{
    public class BookingController : Controller
    {
        private readonly EventEasePOE2_0Context _context;
        private readonly BookingService _bookingService;

        public BookingController(EventEasePOE2_0Context context, BookingService bookingService)
        {
            _context = context;
            _bookingService = bookingService;
        }

        public async Task<IActionResult> Index()
        {
            var bookings = await _context.Bookings
                .Include(b => b.Event).ThenInclude(e => e.EventType)
                .Include(b => b.Venue)
                .ToListAsync();
            return View(bookings); // Will look for Views/Bookings/Index.cshtml
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
