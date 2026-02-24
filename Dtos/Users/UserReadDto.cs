using WeddingPlanner.Api.Models.Enums;

namespace WeddingPlanner.Api.Dtos.Users
{
    public class UserReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserRole Role { get; set; }
    }
}
