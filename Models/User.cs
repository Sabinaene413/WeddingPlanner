using WeddingPlanner.Api.Models.Enums;

namespace WeddingPlanner.Api.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Username { get; set; } = null!; // Folosit la login
        public string PasswordHash { get; set; } = null!; // Parola criptată


        public UserRole Role { get; set; }

        public ICollection<Wedding> Weddings { get; set; } = new List<Wedding>();

    }
}
