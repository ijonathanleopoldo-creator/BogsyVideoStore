
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BogsyVideoStore.Data;
using BogsyVideoStore.Models;

namespace BogsyVideoStore.Controllers
{
    [Authorize]
    public class VideosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VideosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Videos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Videos.ToListAsync());
        }

        // GET: Videos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Videos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Category,MaxRentalDays,TotalCopies")] Video video)
        {
            if (ModelState.IsValid)
            {
                _context.Add(video);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Video title added successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(video);
        }

        // GET: Videos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var video = await _context.Videos.FindAsync(id);
            if (video == null) return NotFound();

            return View(video);
        }

        // POST: Videos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Category,MaxRentalDays,TotalCopies")] Video video)
        {
            if (id != video.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(video);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Video updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Videos.Any(e => e.Id == video.Id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(video);
        }

        // GET: Videos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var video = await _context.Videos.FirstOrDefaultAsync(m => m.Id == id);
            if (video == null) return NotFound();

            return View(video);
        }

        // POST: Videos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var video = await _context.Videos.FindAsync(id);
            if (video != null)
            {
                // Check if any video copy is currently rented out
                bool isRented = await _context.Rentals.AnyAsync(r => r.VideoId == id && !r.IsReturned);
                if (isRented)
                {
                    TempData["Error"] = "Cannot delete video while copies are currently rented out!";
                    return RedirectToAction(nameof(Index));
                }

                _context.Videos.Remove(video);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Video deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
