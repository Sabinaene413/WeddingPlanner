using WeddingPlanner.Api.Dtos.WeddingTasks;

namespace WeddingPlanner.Api.Dtos.Weddings
{
    public class WeddingReadDto
    {
        public int Id { get; set; }             
        public string Title { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public bool IsArchived { get; set; }

        // lista taskurilor
        public List<WeddingTaskReadDto> Tasks { get; set; } = new();
    }
}
