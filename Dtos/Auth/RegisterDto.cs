using WeddingPlanner.Api.Models.Enums;

namespace WeddingPlanner.Api.Dtos.Auth
{
    public class RegisterDto
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
}
