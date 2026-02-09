namespace WeddingPlanner.Api.Models
{
    public class Guest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsConfirmed { get; set; }
        public int WeddingId { get; set; }
    }
}
