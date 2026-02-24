namespace WeddingPlanner.Api.Dtos.Weddings
{
    public class WeddingUpdateDto
    {
        public string Title { get; set; } = null!;
        public DateTime Date { get; set; }
        public string Location { get; set; } = null!;
    }
}
