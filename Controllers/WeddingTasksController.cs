using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WeddingPlanner.Api.Data;
using WeddingPlanner.Api.Dtos.WeddingTasks;
using WeddingPlanner.Api.Infrastructure.Auth;
using WeddingPlanner.Api.Models;
using WeddingPlanner.Api.Models.Enums;
using WeddingPlanner.Api.Services.Permissions;

namespace WeddingPlanner.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WeddingTasksController : ControllerBase
    {
        private readonly WeddingPlannerContext _context;
        private readonly IWeddingPermissionService _permissions;

        public WeddingTasksController(WeddingPlannerContext context,
               IWeddingPermissionService permissions)
        {
            _context = context;
            _permissions = permissions;
        }
        // GET /api/weddingTasks
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] bool? completed,
            [FromQuery] int? weddingId)
        {
            var tasks = _context.WeddingTasks.AsNoTracking();

            if (completed.HasValue)
            {
                tasks = tasks.Where(t => t.IsCompleted == completed.Value);
            }

            if (weddingId.HasValue)
            {
                tasks = tasks.Where(t => t.WeddingId == weddingId.Value);
            }

            var dtos = await tasks
                .Select(t=> MapToReadDto(t))
                .ToListAsync();

            return Ok(dtos);
        }

        // GET /api/weddingTasks/wedding/{weddingId}
        [HttpGet("wedding/{weddingId}")]
        public async Task<IActionResult> GetByWedding(int weddingId)
        {
            var dtos = await _context.WeddingTasks
                .Where(t => t.WeddingId == weddingId)
                .Select(t => MapToReadDto(t))
                .ToListAsync();

            return Ok(dtos);
        }


        // GET /api/weddingTasks/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _context.WeddingTasks.FirstOrDefaultAsync(w => w.Id == id);
            if (task == null) return NotFound();

            return Ok(MapToReadDto(task));
        }

        // POST /api/weddingTasks
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WeddingTaskCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = HttpContext.GetCurrentUser();

            var wedding = await _context.Weddings.FirstOrDefaultAsync(w => w.Id == dto.WeddingId);
            if (wedding == null)
                return BadRequest($"Wedding with id {dto.WeddingId} does not exist.");

            if (!_permissions.CanManageTasks(user, wedding))
                return Forbid();

            var task = new WeddingTask
            {
                Title = dto.Title,
                WeddingId = dto.WeddingId,
                IsCompleted = false
            };

            _context.WeddingTasks.Add(task);
            await _context.SaveChangesAsync();
            await UpdateWeddingStatus(task.WeddingId);

            return
              CreatedAtAction(
                nameof(GetById),
                new { id = task.Id },
                MapToReadDto(task)
              );
        }

        // PUT /api/weddingtasks/{id}/complete
        [HttpPut("{id}/complete")]
        public async Task<IActionResult> UpdateCompletion(int id, [FromBody] WeddingTaskUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = HttpContext.GetCurrentUser();

            if (!_permissions.CanToggleTask(user))
                return Forbid();

            var task = await _context.WeddingTasks
                .Include(t => t.Wedding)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (task == null)
                return NotFound();


            if (task.Wedding.Status == WeddingStatus.Completed)
                return BadRequest("Cannot modify tasks of a completed wedding.");

            task.IsCompleted = dto.IsCompleted;

            var readDto = new WeddingTaskReadDto
            {
                Id = task.Id,
                Title = task.Title,
                IsCompleted = task.IsCompleted
            };
            await _context.SaveChangesAsync();
            await UpdateWeddingStatus(task.WeddingId);

            return Ok(readDto);
        }


        // DELETE /api/weddingtasks/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = HttpContext.GetCurrentUser();

            var task = await _context.WeddingTasks
                .Include(w => w.Wedding)
                .FirstOrDefaultAsync(t =>
            t.Id == id);
            if (task == null)
                return NotFound();

            if (!_permissions.CanManageTasks(user, task.Wedding))
                return Forbid();

            var weddingId = task.WeddingId;

            _context.WeddingTasks.Remove(task);
            await _context.SaveChangesAsync();

            await UpdateWeddingStatus(weddingId);

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

        private async Task UpdateWeddingStatus(int weddingId)
        {
            var wedding = await _context.Weddings
                .Include(w => w.Tasks)
                .FirstOrDefaultAsync(w => w.Id == weddingId);

            if (wedding == null)
                return;

            if (!wedding.Tasks.Any())
            {
                wedding.Status = WeddingStatus.Planned;
            }
            else
            {
                wedding.Status = WeddingStatus.InProgress;
            }

            await _context.SaveChangesAsync();
        }

    }

}