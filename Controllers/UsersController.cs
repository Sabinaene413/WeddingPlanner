using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingPlanner.Api.Data;
using WeddingPlanner.Api.Dtos.Users;
using WeddingPlanner.Api.Models;

namespace WeddingPlanner.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly WeddingPlannerContext _context;

        public UsersController(WeddingPlannerContext context)
        {
            _context = context;
        }

        // GET /api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserReadDto>>> GetAll()
        {
            var users = await _context.Users
                .Select(u => new UserReadDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Role = u.Role
                })
                .ToListAsync();

            return Ok(users);
        }

        // GET /api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserReadDto>> GetById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            return Ok(new UserReadDto
            {
                Id = user.Id,
                Name = user.Name,
                Role = user.Role
            });
        }

        // POST /api/users
        [HttpPost]
        public async Task<ActionResult<UserReadDto>> Create(UserCreateDto dto)
        {
            var user = new User
            {
                Name = dto.Name,
                Role = dto.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var result = new UserReadDto
            {
                Id = user.Id,
                Name = user.Name,
                Role = user.Role
            };

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, result);
        }

        // PUT /api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UserUpdateDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.Name = dto.Name;
            user.Role = dto.Role;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE /api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
