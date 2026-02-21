using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingPlanner.Api.Data;
using WeddingPlanner.Api.Dtos.Weddings;
using WeddingPlanner.Api.Dtos.WeddingTasks;
using WeddingPlanner.Api.Models;
using WeddingPlanner.Api.Models.Enums;

namespace WeddingPlanner.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class WeddingsController : ControllerBase
    {
        private readonly WeddingPlannerContext _context;

        public WeddingsController(WeddingPlannerContext context)
        {
            _context = context;
        }

        // GET /api/weddings
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var weddingDtos = await _context.Weddings
                .AsNoTracking()
                .Select(w => new WeddingReadDto
                {
                    Id = w.Id,
                    Title = w.Title,
                    Date = w.Date,
                    Location = w.Location,
                    Status = w.Status.ToString(),
                    Tasks = w.Tasks.Select(t => new WeddingTaskReadDto
                    {
                        Id = t.Id,
                        Title = t.Title,
                        IsCompleted = t.IsCompleted
                    }).ToList()
                })
                .ToListAsync();

            return Ok(weddingDtos);
        }

        // GET /api/weddings/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var wedding = await _context.Weddings
                .AsNoTracking()
                .Where(w => w.Id == id)
                .Select(w => new WeddingReadDto
                {
                    Id = w.Id,
                    Title = w.Title,
                    Date = w.Date,
                    Location = w.Location,
                    Status = w.Status.ToString(),
                    Tasks = w.Tasks.Select(t => new WeddingTaskReadDto
                    {
                        Id = t.Id,
                        Title = t.Title,
                        IsCompleted = t.IsCompleted
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (wedding == null)
                return NotFound();

            return Ok(wedding);
        }

        // POST /api/weddings
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WeddingCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var wedding = new Wedding
            {
                Title = dto.Title,
                Date = dto.Date,
                Location = dto.Location,
            };

            await _context.Weddings.AddAsync(wedding);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = wedding.Id }, wedding);
        }

        // GET /api/weddings/{id}/tasks
        [HttpGet("{id}/tasks")]
        public async Task<IActionResult> GetTasksForWedding(int id)
        {
            var wedding = await _context.Weddings.FirstOrDefaultAsync(w => w.Id == id);
            if (wedding == null)
                return NotFound($"Wedding with id {id} not found.");

            var tasks = await _context.WeddingTasks
                .Where(t => t.WeddingId == id)
                .ToListAsync();

            return Ok(tasks);
        }

        // PUT /api/weddings/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] WeddingCreateDto dto)
        {
            var wedding = await _context.Weddings.FindAsync(id);
            if (wedding == null)
                return NotFound();

            wedding.Title = dto.Title;
            wedding.Date = dto.Date;
            wedding.Location = dto.Location;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}/complete")]
        public async Task<IActionResult> MarkAsCompleted(int id)
        {
            var wedding = await _context.Weddings
                .Include(w => w.Tasks)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (wedding == null)
                return NotFound();

            if (!wedding.Tasks.Any())
                return BadRequest("Cannot complete a wedding without tasks.");

            if (!wedding.Tasks.All(t => t.IsCompleted))
                return BadRequest("All tasks must be completed before marking the wedding as completed.");

            wedding.Status = WeddingStatus.Completed;
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE /api/weddings/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var wedding = await _context.Weddings
                .Include(w => w.Tasks)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (wedding == null)
                return NotFound();

            if (wedding.Tasks.Any())
                return BadRequest("Cannot delete wedding with existing tasks.");

            _context.Weddings.Remove(wedding);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
