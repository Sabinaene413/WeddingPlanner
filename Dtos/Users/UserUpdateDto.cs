using WeddingPlanner.Api.Models.Enums;

namespace WeddingPlanner.Api.Dtos.Users
{
    public class UserUpdateDto
    {
        public string Name { get; set; }
        public UserRole Role { get; set; }
    }
}
