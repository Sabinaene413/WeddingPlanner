using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingPlanner.Api.Data;
using WeddingPlanner.Api.Dtos.Auth;
using WeddingPlanner.Api.Models;
using WeddingPlanner.Api.Models.Enums;
using WeddingPlanner.Api.Services.Auth;

namespace WeddingPlanner.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly WeddingPlannerContext _context;
        private readonly IJwtTokenService _jwtService;
        public AuthController(WeddingPlannerContext context, IJwtTokenService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (await _context.Users.AnyAsync(u => u.Username == model.Username))
                return BadRequest("Utilizatorul există deja.");

            if (model.Role is not (UserRole.BrideGroom or UserRole.Organizer))
                return BadRequest("Rol invalid.");

            var user = new User
            {
                Name = model.Name,
                Username = model.Username,
                Role = model.Role,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Înregistrare reușită!" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                return Unauthorized("Username sau parolă incorectă.");

            var token = _jwtService.Generate(user);
            return Ok(new { token });
        }

    }
}
