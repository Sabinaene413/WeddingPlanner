using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeddingPlanner.Api.Dtos.WeddingTasks;
using WeddingPlanner.Api.Infrastructure.Auth;
using WeddingPlanner.Api.Models.Enums;
using WeddingPlanner.Api.Services.Permissions;
using WeddingPlanner.Api.Services.Weddings;
using WeddingPlanner.Api.Services.WeddingTasks;

namespace WeddingPlanner.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WeddingTasksController : ControllerBase
    {
        private readonly IWeddingTaskService _taskService;
        private readonly IWeddingService _weddingService;
        private readonly IWeddingPermissionService _permissions;

        public WeddingTasksController(IWeddingTaskService taskService,
               IWeddingService weddingService,
               IWeddingPermissionService permissions)
        {
            _taskService = taskService;
            _weddingService = weddingService;
            _permissions = permissions;
        }

        // GET /api/weddingTasks
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] bool? completed,
            [FromQuery] int? weddingId)
        {
            var tasks = await _taskService.GetAllAsync(completed, weddingId);
            return Ok(tasks);
        }

        // GET /api/weddingTasks/wedding/{weddingId}
        [HttpGet("wedding/{weddingId}")]
        public async Task<IActionResult> GetByWedding(int weddingId)
        {
            var tasks = await _taskService.GetByWeddingIdAsync(weddingId);
            return Ok(tasks);
        }

        // GET /api/weddingTasks/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _taskService.GetByIdAsync(id);
            if (task == null) return NotFound();

            return Ok(task);
        }

        // POST /api/weddingTasks
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WeddingTaskCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = HttpContext.GetCurrentUser();

            var wedding = await _weddingService.GetEntityByIdAsync(dto.WeddingId);
            if (wedding == null)
                return BadRequest($"Wedding with id {dto.WeddingId} does not exist.");

            if (!_permissions.CanManageTasks(user, wedding))
                return Forbid();

            var task = await _taskService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
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

            var task = await _taskService.GetEntityByIdAsync(id);
            if (task == null)
                return NotFound();

            if (task.Wedding.Status == WeddingStatus.Completed)
                return BadRequest("Cannot modify tasks of a completed wedding.");

            await _taskService.UpdateCompletionAsync(task, dto.IsCompleted);

            var updatedTask = await _taskService.GetByIdAsync(id);

            return Ok(updatedTask);
        }

        // DELETE /api/weddingtasks/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = HttpContext.GetCurrentUser();

            var task = await _taskService.GetEntityByIdAsync(id);
            if (task == null)
                return NotFound();

            if (!_permissions.CanManageTasks(user, task.Wedding))
                return Forbid();

            await _taskService.DeleteAsync(task);

            return NoContent();
        }
    }
}
