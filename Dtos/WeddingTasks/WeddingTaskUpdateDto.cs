using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Api.Dtos.WeddingTasks
{
    public class WeddingTaskUpdateDto
    {
        [Required]
        public bool IsCompleted { get; set; }
    }
}
