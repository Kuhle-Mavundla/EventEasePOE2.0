using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventEasePOE2._0.Data;
using EventEasePOE2._0.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EventEasePOE2._0.Controllers
{
    // This controller helps people search for venues and events
    public class SearchController : Controller
    {
        // This talks to our database
        private readonly EventEasePOE2_0Context _context;

        // We tell the computer to use our database here
        public SearchController(EventEasePOE2_0Context context)
        {
            _context = context;
        }

        // Show the search page with dropdowns for event types and venues
        public IActionResult Search()
        {
            ViewBag.EventTypes = new SelectList(_context.EventTypes, "EventTypeId", "Name");
            ViewBag.Venues = new SelectList(_context.Venues, "VenueId", "Name");
            return View(); // Show the search form page
        }

        // This runs when someone submits the search form
        [HttpPost]
        public async Task<IActionResult> SearchResults(int? eventTypeId, int? venueId, DateTime? startDate, DateTime? endDate)
        {
            // Start by getting all venues with their events
            var venuesQuery = _context.Venues.Include(v => v.Events).AsQueryable();

            // If the user picked a venue, only show that one
            if (venueId.HasValue && venueId.Value > 0)
                venuesQuery = venuesQuery.Where(v => v.VenueId == venueId);

            // If the user picked an event type, only show venues that have events of that type
            if (eventTypeId.HasValue && eventTypeId.Value > 0)
            {
                venuesQuery = venuesQuery.Where(v =>
                    v.Events.Any(e => e.EventTypeId == eventTypeId));
            }

            // If the user picked a start and end date to check availability
            if (startDate.HasValue && endDate.HasValue && startDate <= endDate)
            {
                var bookings = _context.Bookings.AsQueryable();

                // Find venues that are already booked during that time
                var bookedVenueIds = bookings
                    .Where(b =>
                        b.StartDate >= startDate && b.EndDate <= endDate)
                    .Select(b => b.VenueId)
                    .Distinct();

                // Take out venues that are booked so only free ones show
                venuesQuery = venuesQuery.Where(v => !bookedVenueIds.Contains(v.VenueId));
            }

            // Get the final list of venues that match the search
            var results = await venuesQuery.ToListAsync();

            // Send data back so dropdowns remember what was picked
            ViewBag.EventTypes = new SelectList(_context.EventTypes, "EventTypeId", "Name", eventTypeId);
            ViewBag.Venues = new SelectList(_context.Venues, "VenueId", "Name", venueId);
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");

            return View(results); // Show the search results page with these venues
        }
    }
}
