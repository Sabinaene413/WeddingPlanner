using WeddingPlanner.Api.Models.Enums;

namespace WeddingPlanner.Api.Infrastructure.Auth
{
    public record CurrentUser(int Id, UserRole Role);
}
