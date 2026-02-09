namespace WeddingPlanner.Api.Models
{
    public class WeddingTask
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public int WeddingId { get; set; }
    }
}
