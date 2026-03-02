using WeddingPlanner.Api.Models;

namespace WeddingPlanner.Api.Services.Auth
{
    public interface IJwtTokenService
    {
        string Generate(User user);
    }
}
