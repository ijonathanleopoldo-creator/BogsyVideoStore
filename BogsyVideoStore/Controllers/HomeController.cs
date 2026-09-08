using BogsyVideoStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BogsyVideoStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // If user is logged in, redirect directly to the main BVS module
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Rentals");
            }

            // If unauthenticated, redirect directly to ASP.NET Core Identity Login
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }

        /*
        public IActionResult Index()
        {
            return View();
        }
        */
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
