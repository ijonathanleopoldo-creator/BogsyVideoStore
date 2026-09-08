using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BogsyVideoStore.Models
{
    public class Video
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Video title is required.")]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public VideoCategory Category { get; set; }

        [Required]
        [Range(1, 3, ErrorMessage = "Max rental duration must be between 1 and 3 days.")]
        [Display(Name = "Max Days Allowed")]
        public int MaxRentalDays { get; set; } = 3;

        [Required]
        [Range(1, 999, ErrorMessage = "Total copies must be at least 1.")]
        [Display(Name = "Total Stock Copies")]
        public int TotalCopies { get; set; } = 1;

        // Auto-calculated rental fee: VCD = 25 Pesos, DVD = 50 Pesos
        [NotMapped]
        public decimal RentalPrice => Category == VideoCategory.VCD ? 25.00m : 50.00m;

        public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
    }
}

