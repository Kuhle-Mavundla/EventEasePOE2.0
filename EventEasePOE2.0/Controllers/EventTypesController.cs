using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventEasePOE2._0.Data;
using EventEasePOE2._0.Models;

namespace EventEasePOE2._0.Controllers
{
    // This controller helps us manage event types (like categories of events)
    public class EventTypeController : Controller
    {
        // This talks to the database for event types
        private readonly EventEasePOE2_0Context _context;

        // We set up the database helper here
        public EventTypeController(EventEasePOE2_0Context context)
        {
            _context = context;
        }

        // Show a list of all event types
        public async Task<IActionResult> Index() => View(await _context.EventTypes.ToListAsync());

        // Show a form to make a new event type
        public IActionResult Create() => View();

        // When form is sent, save the new event type if everything is okay
        [HttpPost]
        public async Task<IActionResult> Create(EventType type)
        {
            if (ModelState.IsValid)
            {
                _context.EventTypes.Add(type);  // Add the new event type
                await _context.SaveChangesAsync(); // Save it to the database
                return RedirectToAction(nameof(Index)); // Go back to list page
            }
            return View(type); // Show form again if something is wrong
        }

        // Show a form to edit an event type
        public async Task<IActionResult> Edit(int id)
        {
            var type = await _context.EventTypes.FindAsync(id); // Find event type by id
            return type == null ? NotFound() : View(type); // Show form or error if not found
        }

        // When form is sent, update the event type if everything is okay
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EventType type)
        {
            if (id != type.EventTypeId) return NotFound(); // If IDs don’t match, show error

            if (ModelState.IsValid)
            {
                _context.Update(type); // Update event type
                await _context.SaveChangesAsync(); // Save changes
                return RedirectToAction(nameof(Index)); // Go back to list
            }
            return View(type); // Show form again if something is wrong
        }

        // Show a page to confirm deleting an event type
        public async Task<IActionResult> Delete(int id)
        {
            var type = await _context.EventTypes.FindAsync(id); // Find event type by id
            return type == null ? NotFound() : View(type); // Show confirmation or error
        }

        // When delete is confirmed, remove the event type
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var type = await _context.EventTypes.FindAsync(id); // Find event type by id
            if (type == null)
            {
                return NotFound(); // If not found, show error
            }
            _context.EventTypes.Remove(type); // Delete it from the database
            await _context.SaveChangesAsync(); // Save changes
            return RedirectToAction(nameof(Index)); // Go back to list
        }

    }
}
