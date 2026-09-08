using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BogsyVideoStore.Data;
using BogsyVideoStore.Models; // Required for VideoCategory
using BogsyVideoStore.Models.ViewModels; // Required for ViewModels

namespace BogsyVideoStore.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // REPORT 1: List of videos in alphabetical order with Quantity IN and OUT
        public async Task<IActionResult> VideoInventoryReport()
        {
            var videos = await _context.Videos
                .Include(v => v.Rentals)
                .OrderBy(v => v.Title)
                .ToListAsync();

            var reportList = new List<VideoInventoryReportViewModel>();

            foreach (var v in videos)
            {
                int countOut = v.Rentals.Count(r => !r.IsReturned);
                int countIn = v.TotalCopies - countOut;

                reportList.Add(new VideoInventoryReportViewModel
                {
                    Title = v.Title,
                    Category = v.Category,
                    QuantityIn = Math.Max(0, countIn),
                    QuantityOut = countOut
                });
            }

            return View(reportList);
        }

        // REPORT 2: List of customers and the videos they are currently renting
        public async Task<IActionResult> CustomerActiveRentalsReport()
        {
            var customers = await _context.Customers
                .Include(c => c.Rentals.Where(r => !r.IsReturned))
                .ThenInclude(r => r.Video)
                .Where(c => c.Rentals.Any(r => !r.IsReturned))
                .OrderBy(c => c.FullName)
                .ToListAsync();

            var reportList = customers.Select(c => new CustomerActiveRentalsViewModel
            {
                CustomerId = c.Id,
                CustomerName = c.FullName,
                ContactNumber = c.ContactNumber,
                ActiveRentals = c.Rentals.Select(r => {
                    int overdueDays = (DateTime.Today - r.DueDate.Date).Days;
                    return new ActiveRentalItemViewModel
                    {
                        RentalId = r.Id,
                        VideoTitle = r.Video?.Title ?? "Unknown",
                        Category = r.Video?.Category ?? Models.VideoCategory.VCD,
                        RentDate = r.RentDate,
                        DueDate = r.DueDate,
                        DaysOverdue = overdueDays > 0 ? overdueDays : 0,
                        OverdueFee = overdueDays > 0 ? overdueDays * 5.00m : 0.00m
                    };
                }).ToList()
            }).ToList();

            return View(reportList);
        }
    }
}
