using Microsoft.AspNetCore.Mvc;
using WeddingPlanner.Api.Data;
using WeddingPlanner.Api.Dtos.WeddingTasks;
using WeddingPlanner.Api.Models;

namespace WeddingPlanner.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeddingTasksController : ControllerBase
    {
        // GET /api/weddingTasks
        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] bool? completed,
            [FromQuery] int? weddingId)
        {
            var tasks = InMemoryData.WeddingTasks.AsQueryable();

            if (completed.HasValue)
            {
                tasks = tasks.Where(t => t.IsCompleted == completed.Value);
            }

            if (weddingId.HasValue)
            {
                tasks = tasks.Where(t => t.WeddingId == weddingId.Value);
            }

            var dtos = tasks
                .Select(MapToReadDto)
                .ToList();

            return Ok(dtos);
        }

        // GET /api/weddingTasks/wedding/{weddingId}
        [HttpGet("wedding/{weddingId}")]
        public IActionResult GetByWedding(int weddingId)
        {
            var dtos = InMemoryData.WeddingTasks
                .Where(t => t.WeddingId == weddingId)
                .Select(MapToReadDto)
                .ToList();

            return Ok(dtos);
        }


        // GET /api/weddingTasks/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var task = InMemoryData.WeddingTasks.FirstOrDefault(w => w.Id == id);
            if (task == null) return NotFound();

            return Ok(MapToReadDto(task));
        }

        // POST /api/weddingTasks
        [HttpPost]
        public IActionResult Create([FromBody] WeddingTaskCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var weddingExists = InMemoryData.Weddings.Any(w => w.Id == dto.WeddingId);
            if (!weddingExists)
                return BadRequest($"Wedding with id {dto.WeddingId} does not exist.");

            var task = new WeddingTask
            {
                Id = InMemoryData.NextTaskId(),
                Title = dto.Title,
                WeddingId = dto.WeddingId,
                IsCompleted = false
            };

            InMemoryData.WeddingTasks.Add(task);

            return CreatedAtAction(
              nameof(GetById),
              new { id = task.Id },
              MapToReadDto(task)
             );
        }

        // PUT /api/weddingtasks/{id}/complete
        [HttpPut("{id}/complete")]
        public IActionResult UpdateCompletion(int id, [FromBody] WeddingTaskUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var task = InMemoryData.WeddingTasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
                return NotFound();

            task.IsCompleted = dto.IsCompleted;

            var readDto = new WeddingTaskReadDto
            {
                Id = task.Id,
                Title = task.Title,
                IsCompleted = task.IsCompleted
            };

            return Ok(readDto);
        }

        // DELETE /api/weddingtasks/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var task = InMemoryData.WeddingTasks.FirstOrDefault(t =>
            t.Id == id);
            if (task == null)
                return NotFound();

            InMemoryData.WeddingTasks.Remove(task);

            return NoContent();
        }

        private static WeddingTaskReadDto MapToReadDto(WeddingTask task)
        {
            return new WeddingTaskReadDto
            {
                Id = task.Id,
                Title = task.Title,
                IsCompleted = task.IsCompleted
            };
        }

    }

}