using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventEasePOE2._0.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using EventEasePOE2._0.Data;

namespace EventEasePOE.Controllers
{
    public class EventsController : Controller
    {
        private readonly EventEasePOE2_0Context _context;

        public EventsController(EventEasePOE2_0Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var events = await _context.Events.Include(e => e.EventType).Include(e => e.Venue).ToListAsync();
            return View(events); // Will look for Views/Events/Index.cshtml
        }


        public IActionResult Create()
        {
            ViewBag.Venues = new SelectList(_context.Venues, "VenueId", "Name");
            ViewBag.EventTypes = new SelectList(_context.EventTypes, "EventTypeId", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Venues = new SelectList(_context.Venues, "VenueId", "Name", @event.VenueId);
            ViewBag.EventTypes = new SelectList(_context.EventTypes, "EventTypeId", "Name", @event.EventTypeId);
            return View(@event);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null) return NotFound();
            ViewBag.Venues = new SelectList(_context.Venues, "VenueId", "Name", @event.VenueId);
            ViewBag.EventTypes = new SelectList(_context.EventTypes, "EventTypeId", "Name", @event.EventTypeId);
            return View(@event);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Event @event)
        {
            if (id != @event.EventId) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Venues = new SelectList(_context.Venues, "VenueId", "Name", @event.VenueId);
            ViewBag.EventTypes = new SelectList(_context.EventTypes, "EventTypeId", "Name", @event.EventTypeId);
            return View(@event);
        }

        public async Task<IActionResult> Details(int id)
        {
            var @event = await _context.Events
                .Include(e => e.Venue)
                .Include(e => e.EventType)
                .FirstOrDefaultAsync(e => e.EventId == id);
            return @event == null ? NotFound() : View(@event);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var @event = await _context.Events
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(m => m.EventId == id);

            if (@event == null) return NotFound();

            return View(@event);
        }

        // POST: Events/DeleteConfirmed/1
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event != null)
            {
                _context.Events.Remove(@event);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}