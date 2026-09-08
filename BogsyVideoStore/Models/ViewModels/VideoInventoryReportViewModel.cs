using BogsyVideoStore.Models; // Required for VideoCategory

namespace BogsyVideoStore.Models.ViewModels
{
    public class VideoInventoryReportViewModel
    {
        public string Title { get; set; } = string.Empty;
        public VideoCategory Category { get; set; }
        public int QuantityIn { get; set; }
        public int QuantityOut { get; set; }
    }
}