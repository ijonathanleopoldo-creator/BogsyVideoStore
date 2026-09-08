using BogsyVideoStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BogsyVideoStore.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // Auto-create database and apply migrations
            context.Database.Migrate();

            // 1. Seed Default Admin Account
            var adminEmail = "admin@bvs.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(adminUser, "Admin123!");
            }

            // 2. Seed Sample Videos and Customers
            if (!context.Videos.Any())
            {
                var customers = new Customer[]
                {
                    new Customer { FullName = "Juan Dela Cruz", ContactNumber = "09171234567", Address = "Buhangin, Davao City" },
                    new Customer { FullName = "Maria Santos", ContactNumber = "09189876543", Address = "Bajada, Davao City" }
                };
                context.Customers.AddRange(customers);
                await context.SaveChangesAsync();

                var videos = new Video[]
                {
                    new Video { Title = "Madagascar", Category = VideoCategory.VCD, MaxRentalDays = 3, TotalCopies = 3 },
                    new Video { Title = "Mr. and Mrs. Smith", Category = VideoCategory.DVD, MaxRentalDays = 3, TotalCopies = 4 }
                };
                context.Videos.AddRange(videos);
                await context.SaveChangesAsync();
            }
        }
    }
}