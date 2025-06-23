using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventEasePOE2._0.Data;
using EventEasePOE2._0.Models;
using EventEasePOE2._0.Services;
using Microsoft.AspNetCore.Http;

namespace EventEasePOE2._0.Controllers
{
    // This controller handles all the venue pages and actions
    public class VenuesController : Controller
    {
        // These help us talk to the database and upload pictures
        private readonly EventEasePOE2_0Context _context;
        private readonly BlobService _blobService;

        // This runs when the controller is created
        public VenuesController(EventEasePOE2_0Context context, BlobService blobService)
        {
            _context = context;      // Connect to the database
            _blobService = blobService;  // Connect to the image uploader
        }

        // Show a list of all venues
        public async Task<IActionResult> Index()
        {
            return View(await _context.Venues.ToListAsync());
        }

        // Show details for one venue by its id
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) // If no id is given, show error
                return NotFound();

            var venue = await _context.Venues.FirstOrDefaultAsync(m => m.VenueId == id);
            if (venue == null) // If venue not found, show error
                return NotFound();

            return View(venue); // Show the venue details page
        }

        // Show the form to create a new venue
        public IActionResult Create()
        {
            return View();
        }

        // When the create form is submitted, save the venue and picture
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VenueId,Name,Location,Capacity,CreatedAt")] Venue venue, IFormFile image)
        {
            if (ModelState.IsValid) // Check if the form info is okay
            {
                if (image != null && image.Length > 0) // If a picture was uploaded
                {
                    var imageUrl = await _blobService.UploadVenueImageAsync(image); // Upload it
                    venue.ImageUrl = imageUrl; // Save the picture URL to the venue
                }

                _context.Add(venue); // Add the venue to the database
                await _context.SaveChangesAsync(); // Save the changes
                return RedirectToAction(nameof(Index)); // Go back to the list
            }

            return View(venue); // If something is wrong, show the form again
        }

        // Show the form to edit an existing venue
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) // If no id is given, show error
                return NotFound();

            var venue = await _context.Venues.FindAsync(id);
            if (venue == null) // If venue not found, show error
                return NotFound();

            return View(venue); // Show the edit form
        }

        // When the edit form is submitted, save the changes
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VenueId,Name,Location,Capacity,ImageUrl,CreatedAt")] Venue venue, IFormFile image)
        {
            if (id != venue.VenueId) // Check if the id matches
                return NotFound();

            if (ModelState.IsValid) // Check if the form info is okay
            {
                try
                {
                    if (image != null && image.Length > 0) // If a new picture was uploaded
                    {
                        var imageUrl = await _blobService.UploadVenueImageAsync(image); // Upload it
                        venue.ImageUrl = imageUrl; // Update the picture URL
                    }

                    _context.Update(venue); // Update the venue info
                    await _context.SaveChangesAsync(); // Save the changes
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VenueExists(venue.VenueId)) // Check if venue still exists
                        return NotFound();
                    else
                        throw; // Something else went wrong, throw error
                }

                return RedirectToAction(nameof(Index)); // Go back to the list
            }

            return View(venue); // If something is wrong, show the form again
        }

        // Show confirmation page before deleting a venue
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) // If no id, show error
                return NotFound();

            var venue = await _context.Venues.FirstOrDefaultAsync(m => m.VenueId == id);
            if (venue == null) // If venue not found, show error
                return NotFound();

            return View(venue); // Show delete confirmation
        }

        // Delete the venue after confirmation
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venue = await _context.Venues
                .Include(v => v.Bookings) // Also get bookings
                .Include(v => v.Events)   // Also get events
                .FirstOrDefaultAsync(v => v.VenueId == id);

            if (venue == null) // If venue not found, show error
                return NotFound();

            // Check if venue has bookings or events and prevent delete if yes
            if (venue.Bookings.Any() || venue.Events.Any())
            {
                TempData["ErrorMessage"] = "Cannot delete venue with existing bookings or events.";
                return RedirectToAction(nameof(Index));
            }

            _context.Venues.Remove(venue); // Remove venue from database
            await _context.SaveChangesAsync(); // Save changes

            TempData["SuccessMessage"] = "Venue deleted successfully.";
            return RedirectToAction(nameof(Index)); // Go back to the list
        }

        // Helper to check if a venue exists by id
        private bool VenueExists(int id)
        {
            return _context.Venues.Any(e => e.VenueId == id);
        }
    }
}
