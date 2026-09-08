
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BogsyVideoStore.Data;
using BogsyVideoStore.Models;

namespace BogsyVideoStore.Controllers
{
    [Authorize]
    public class RentalsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RentalsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Rentals
        public async Task<IActionResult> Index()
        {
            var rentals = await _context.Rentals
                .Include(r => r.Customer)
                .Include(r => r.Video)
                .OrderByDescending(r => r.RentDate)
                .ToListAsync();

            return View(rentals);
        }

        // GET: Rentals/Create
        public async Task<IActionResult> Create()
        {
            ViewData["CustomerId"] = new SelectList(await _context.Customers.OrderBy(c => c.FullName).ToListAsync(), "Id", "FullName");
            ViewData["VideoId"] = new SelectList(await _context.Videos.OrderBy(v => v.Title).ToListAsync(), "Id", "Title");
            return View();
        }

        // POST: Rentals/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int CustomerId, int VideoId, int DaysToRent)
        {
            var video = await _context.Videos.FindAsync(VideoId);
            var customer = await _context.Customers.FindAsync(CustomerId);

            if (video == null || customer == null)
            {
                TempData["Error"] = "Invalid selection.";
                return RedirectToAction(nameof(Create));
            }

            // Check max rental duration rule (1 to 3 days limit on video)
            if (DaysToRent < 1 || DaysToRent > video.MaxRentalDays)
            {
                ModelState.AddModelError("", $"Rental days for '{video.Title}' must be between 1 and {video.MaxRentalDays} days.");
                ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "FullName", CustomerId);
                ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "Title", VideoId);
                return View();
            }

            // Check stock availability (Quantity In)
            int currentlyRentedCount = await _context.Rentals.CountAsync(r => r.VideoId == VideoId && !r.IsReturned);
            int availableStock = video.TotalCopies - currentlyRentedCount;

            if (availableStock <= 0)
            {
                ModelState.AddModelError("", $"No available copies for '{video.Title}'. All copies are currently rented out.");
                ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "FullName", CustomerId);
                ViewData["VideoId"] = new SelectList(_context.Videos, "Id", "Title", VideoId);
                return View();
            }

            var rental = new Rental
            {
                CustomerId = CustomerId,
                VideoId = VideoId,
                RentDate = DateTime.Today,
                DueDate = DateTime.Today.AddDays(DaysToRent),
                RentalFee = video.RentalPrice,
                IsReturned = false
            };

            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Rental registered! Total Fee: ₱{rental.RentalFee:N2}";
            return RedirectToAction(nameof(Index));
        }

        // GET: Rentals/Return/5
        public async Task<IActionResult> Return(int? id)
        {
            if (id == null) return NotFound();

            var rental = await _context.Rentals
                .Include(r => r.Customer)
                .Include(r => r.Video)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (rental == null || rental.IsReturned)
            {
                TempData["Error"] = "Rental record not found or already returned.";
                return RedirectToAction(nameof(Index));
            }

            // Calculate overdue fees: ₱5/day after DueDate
            DateTime returnDate = DateTime.Today;
            int overdueDays = (returnDate - rental.DueDate.Date).Days;
            rental.LateFee = overdueDays > 0 ? overdueDays * 5.00m : 0.00m;
            rental.ReturnDate = returnDate;

            return View(rental);
        }

        // POST: Rentals/Return/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessReturn(int id)
        {
            var rental = await _context.Rentals.FindAsync(id);
            if (rental == null || rental.IsReturned) return NotFound();

            DateTime returnDate = DateTime.Today;
            int overdueDays = (returnDate - rental.DueDate.Date).Days;

            rental.ReturnDate = returnDate;
            rental.LateFee = overdueDays > 0 ? overdueDays * 5.00m : 0.00m;
            rental.IsReturned = true;

            _context.Update(rental);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Video returned successfully. Late Fee Paid: ₱{rental.LateFee:N2}";
            return RedirectToAction(nameof(Index));
        }
    }
}
