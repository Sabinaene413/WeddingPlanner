using WeddingPlanner.Api.Models.Enums;

namespace WeddingPlanner.Api.Infrastructure.Auth
{
    public static class HttpContextUserExtensions
    {
        public static CurrentUser GetCurrentUser(this HttpContext context)
        {
            var userId = int.Parse(context.Request.Headers["X-User-Id"]);
            var role = Enum.Parse<UserRole>(context.Request.Headers["X-User-Role"]);

            return new CurrentUser(userId, role);
        }
    }
}
