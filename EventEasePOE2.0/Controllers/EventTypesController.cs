using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventEasePOE2._0.Data;
using EventEasePOE2._0.Models;

namespace EventEasePOE2._0.Controllers
{
    public class EventTypeController : Controller
    {
        private readonly EventEasePOE2_0Context _context;

        public EventTypeController(EventEasePOE2_0Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index() => View(await _context.EventTypes.ToListAsync());

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(EventType type)
        {
            if (ModelState.IsValid)
            {
                _context.EventTypes.Add(type);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(type);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var type = await _context.EventTypes.FindAsync(id);
            return type == null ? NotFound() : View(type);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EventType type)
        {
            if (id != type.EventTypeId) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(type);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(type);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var type = await _context.EventTypes.FindAsync(id);
            return type == null ? NotFound() : View(type);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var type = await _context.EventTypes.FindAsync(id);
            if (type == null)
            {
                return NotFound();
            }
            _context.EventTypes.Remove(type);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
