using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BogsyVideoStore.Models
{
    public class Rental
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        [Required]
        [Display(Name = "Video")]
        public int VideoId { get; set; }
        public Video? Video { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Rent Date")]
        public DateTime RentDate { get; set; } = DateTime.Today;

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Return Date")]
        public DateTime? ReturnDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Rental Fee")]
        public decimal RentalFee { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Late Fee (₱5/day)")]
        public decimal LateFee { get; set; } = 0.00m;

        [Display(Name = "Status")]
        public bool IsReturned { get; set; } = false;
    }
}
