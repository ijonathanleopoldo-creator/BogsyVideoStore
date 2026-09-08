namespace BogsyVideoStore.Models.ViewModels
{
    public class ActiveRentalItemViewModel
    {
        public int RentalId { get; set; }
        public string VideoTitle { get; set; } = string.Empty;
        public VideoCategory Category { get; set; }
        public DateTime RentDate { get; set; }
        public DateTime DueDate { get; set; }
        public int DaysOverdue { get; set; }
        public decimal OverdueFee { get; set; }
    }
}
