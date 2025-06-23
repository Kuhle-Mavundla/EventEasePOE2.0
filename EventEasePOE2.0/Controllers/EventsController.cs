using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventEasePOE2._0.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using EventEasePOE2._0.Data;

namespace EventEasePOE.Controllers
{
    // This controller handles everything about events
    public class EventsController : Controller
    {
        // This helps us talk to the database
        private readonly EventEasePOE2_0Context _context;

        // This is the setup to get the database helper
        public EventsController(EventEasePOE2_0Context context)
        {
            _context = context;
        }

        // Show all events on a list page
        public async Task<IActionResult> Index()
        {
            // Get all events and their types and venues from the database
            var events = await _context.Events.Include(e => e.EventType).Include(e => e.Venue).ToListAsync();
            return View(events); // Show this list on the page
        }

        // Show a form to create a new event
        public IActionResult Create()
        {
            // Get lists of venues and event types to pick from on the form
            ViewBag.Venues = new SelectList(_context.Venues, "VenueId", "Name");
            ViewBag.EventTypes = new SelectList(_context.EventTypes, "EventTypeId", "Name");
            return View(); // Show the create event form
        }

        // When the form is submitted, try to save the new event
        [HttpPost]
        public async Task<IActionResult> Create(Event @event)
        {
            // If the form is filled out correctly
            if (ModelState.IsValid)
            {
                _context.Add(@event); // Add the event to the database
                await _context.SaveChangesAsync(); // Save changes
                return RedirectToAction(nameof(Index)); // Go back to the list of events
            }
            // If there is a problem, reload the lists and show the form again with errors
            ViewBag.Venues = new SelectList(_context.Venues, "VenueId", "Name", @event.VenueId);
            ViewBag.EventTypes = new SelectList(_context.EventTypes, "EventTypeId", "Name", @event.EventTypeId);
            return View(@event);
        }

        // Show a form to edit an existing event
        public async Task<IActionResult> Edit(int id)
        {
            var @event = await _context.Events.FindAsync(id); // Find event by id
            if (@event == null) return NotFound(); // If not found, show error
            // Load lists for venues and event types with current selections
            ViewBag.Venues = new SelectList(_context.Venues, "VenueId", "Name", @event.VenueId);
            ViewBag.EventTypes = new SelectList(_context.EventTypes, "EventTypeId", "Name", @event.EventTypeId);
            return View(@event); // Show edit form
        }

        // When edit form is submitted, update the event
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Event @event)
        {
            if (id != @event.EventId) return NotFound(); // If IDs don't match, error

            if (ModelState.IsValid)
            {
                _context.Update(@event); // Update the event in the database
                await _context.SaveChangesAsync(); // Save changes
                return RedirectToAction(nameof(Index)); // Go back to the list
            }
            // If form has errors, reload lists and show form again
            ViewBag.Venues = new SelectList(_context.Venues, "VenueId", "Name", @event.VenueId);
            ViewBag.EventTypes = new SelectList(_context.EventTypes, "EventTypeId", "Name", @event.EventTypeId);
            return View(@event);
        }

        // Show details of a single event
        public async Task<IActionResult> Details(int id)
        {
            var @event = await _context.Events
                .Include(e => e.Venue)
                .Include(e => e.EventType)
                .FirstOrDefaultAsync(e => e.EventId == id); // Find event with venue and type
            return @event == null ? NotFound() : View(@event); // Show event or error
        }

        // Show a page to confirm deleting an event
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound(); // If no id, error

            var @event = await _context.Events
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(m => m.EventId == id); // Find event with venue

            if (@event == null) return NotFound(); // If not found, error

            return View(@event); // Show delete confirmation page
        }

        // When delete is confirmed, remove the event
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id); // Find event by id
            if (@event != null)
            {
                _context.Events.Remove(@event); // Delete the event
                await _context.SaveChangesAsync(); // Save changes
            }
            return RedirectToAction(nameof(Index)); // Go back to list
        }
    }
}
