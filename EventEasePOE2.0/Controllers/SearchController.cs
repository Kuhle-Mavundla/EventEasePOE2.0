using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventEasePOE2._0.Data;
using EventEasePOE2._0.Models;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace EventEasePOE2._0.Controllers
{
    public class SearchController : Controller
    {
        private readonly EventEasePOE2_0Context _context;

        public SearchController(EventEasePOE2_0Context context)
        {
            _context = context;
        }

        public IActionResult Search()
        {
            ViewBag.EventTypes = new SelectList(_context.EventTypes, "EventTypeId", "Name");
            ViewBag.Venues = new SelectList(_context.Venues, "VenueId", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SearchResults(int? eventTypeId, int? venueId, DateTime? startDate, DateTime? endDate)
        {
            // Base query for venues
            var venuesQuery = _context.Venues.Include(v => v.Events).AsQueryable();

            // Filter by venue if selected
            if (venueId.HasValue && venueId.Value > 0)
                venuesQuery = venuesQuery.Where(v => v.VenueId == venueId);

            // Filter by event type if selected
            if (eventTypeId.HasValue && eventTypeId.Value > 0)
            {
                venuesQuery = venuesQuery.Where(v =>
                    v.Events.Any(e => e.EventTypeId == eventTypeId));
            }

            // Filter by availability date range
            if (startDate.HasValue && endDate.HasValue && startDate <= endDate)
            {
                var bookings = _context.Bookings.AsQueryable();

                // Get venue IDs that have bookings overlapping the search date range
                var bookedVenueIds = bookings
                    .Where(b =>
                        b.StartDate >= startDate && b.EndDate <= endDate)
                    .Select(b => b.VenueId)
                    .Distinct();

                // Exclude venues that are booked in the range
                venuesQuery = venuesQuery.Where(v => !bookedVenueIds.Contains(v.VenueId));
            }

            var results = await venuesQuery.ToListAsync();

            ViewBag.EventTypes = new SelectList(_context.EventTypes, "EventTypeId", "Name", eventTypeId);
            ViewBag.Venues = new SelectList(_context.Venues, "VenueId", "Name", venueId);
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");

            return View(results);
        }
    }
}
