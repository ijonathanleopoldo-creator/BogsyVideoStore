namespace BogsyVideoStore.Models.ViewModels
{
    public class CustomerActiveRentalsViewModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public List<ActiveRentalItemViewModel> ActiveRentals { get; set; } = new();
    }
}
