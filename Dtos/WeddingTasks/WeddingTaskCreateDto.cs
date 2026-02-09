using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Api.Dtos.WeddingTasks
{
    public class WeddingTaskCreateDto
    {
        [Required]
        public string Title { get; set; }
        public int WeddingId { get; set; }
    }
}
