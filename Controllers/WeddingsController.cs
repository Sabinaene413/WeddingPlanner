using Microsoft.AspNetCore.Mvc;
using WeddingPlanner.Api.Data;
using WeddingPlanner.Api.Dtos.Weddings;
using WeddingPlanner.Api.Dtos.WeddingTasks;
using WeddingPlanner.Api.Models;

namespace WeddingPlanner.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeddingsController : ControllerBase
    {

        // GET /api/weddings
        [HttpGet]
        public IActionResult GetAll()
        {
            var weddingDtos = InMemoryData.Weddings.Select(w => new WeddingReadDto
            {
                Id = w.Id,
                Title = w.Title,
                Date = w.Date,
                Location = w.Location,
                Tasks = InMemoryData.WeddingTasks
                .Where(t => t.WeddingId == w.Id)
                .Select(t => new WeddingTaskReadDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    IsCompleted = t.IsCompleted
                }).ToList()
            }).ToList();

            return Ok(weddingDtos);
        }

        // GET /api/weddings/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var wedding = InMemoryData.Weddings.FirstOrDefault(w => w.Id == id);
            if (wedding == null) return NotFound();

            var dto = new WeddingReadDto
            {
                Id = wedding.Id,
                Title = wedding.Title,
                Date = wedding.Date,
                Location = wedding.Location,
                Tasks = InMemoryData.WeddingTasks
                .Where(t => t.WeddingId == wedding.Id)
                .Select(t => new WeddingTaskReadDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    IsCompleted = t.IsCompleted
                }).ToList()
            };
            return Ok(dto);
        }

        // POST /api/weddings
        [HttpPost]
        public IActionResult Create([FromBody] WeddingCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var wedding = new Wedding
            {
                Id = InMemoryData.NextWeddingId(),
                Title = dto.Title,
                Date = dto.Date,
                Location = dto.Location,
            };

            InMemoryData.Weddings.Add(wedding);
            return CreatedAtAction(nameof(GetById), new { id = wedding.Id }, wedding);
        }

        // GET /api/weddings/{id}/tasks
        [HttpGet("{id}/tasks")]
        public IActionResult GetTasksForWedding(int id)
        {
            var wedding = InMemoryData.Weddings.FirstOrDefault(w => w.Id == id);
            if (wedding == null)
                return NotFound($"Wedding with id {id} not found.");

            var tasks = InMemoryData.WeddingTasks
                .Where(t => t.WeddingId == id)
                .ToList();

            return Ok(tasks);
        }
    }
}
