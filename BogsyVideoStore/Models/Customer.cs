using System.ComponentModel.DataAnnotations;

namespace BogsyVideoStore.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Customer name is required.")]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contact number is required.")]
        [Phone]
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Address { get; set; }

        public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
    }
}
