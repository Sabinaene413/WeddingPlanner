using System.Security.Claims;
using WeddingPlanner.Api.Models.Enums;

namespace WeddingPlanner.Api.Infrastructure.Auth
{
    public static class HttpContextUserExtensions
    {
        public static CurrentUser GetCurrentUser(this HttpContext context)
        {
            var user = context.User;

            if (user?.Identity?.IsAuthenticated != true)
                throw new UnauthorizedAccessException("User is not authenticated.");

            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)
                              ?? user.FindFirst("sub");

            var roleClaim = user.FindFirst(ClaimTypes.Role)
                  ?? user.FindFirst("role");

            var nameClaim = user.FindFirst(ClaimTypes.Name) ?? user.FindFirst("name") ??
                user.FindFirst("unique_name");

            if (userIdClaim == null || roleClaim == null)
                throw new UnauthorizedAccessException("Missing JWT claims.");

            return new CurrentUser(
                 int.Parse(userIdClaim.Value),
                 nameClaim?.Value ?? "Unknown User",
                 Enum.Parse<UserRole>(roleClaim.Value)
                 );
        }
    }
}
