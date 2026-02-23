using WeddingPlanner.Api.Models.Enums;

namespace WeddingPlanner.Api.Models
{
    public class Wedding
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Location { get; set; } = string.Empty;
        public WeddingStatus Status { get; set; } = WeddingStatus.Planned;
        public bool IsArchived { get; set; } = false;


        // relatia cu taskurile
        public List<WeddingTask> Tasks { get; set; }
    }
}
