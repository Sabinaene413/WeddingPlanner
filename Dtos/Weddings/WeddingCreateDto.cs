using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Api.Dtos.Weddings
{
    public class WeddingCreateDto
    {
        [Required]
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public bool IsSelfManaged { get; set; }
    }
}
