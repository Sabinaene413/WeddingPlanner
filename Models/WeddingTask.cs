namespace WeddingPlanner.Api.Models
{
    public class WeddingTask
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }

        // relatia cu nunta
        public int WeddingId { get; set; }
        public Wedding Wedding { get; set; }
    }
}
