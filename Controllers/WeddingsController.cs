using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeddingPlanner.Api.Dtos.Weddings;
using WeddingPlanner.Api.Infrastructure.Auth;
using WeddingPlanner.Api.Models.Enums;
using WeddingPlanner.Api.Services.Permissions;
using WeddingPlanner.Api.Services.Weddings;

namespace WeddingPlanner.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WeddingsController : ControllerBase
    {
        private readonly IWeddingService _weddingService;
        private readonly IWeddingPermissionService _permissions;

        public WeddingsController(IWeddingService weddingService,
            IWeddingPermissionService permissions)
        {
            _weddingService = weddingService;
            _permissions = permissions;
        }

        // GET /api/weddings
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool? archived)
        {
            var weddings = await _weddingService.GetAllAsync(archived);
            return Ok(weddings);
        }

        // GET /api/weddings/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var wedding = await _weddingService.GetByIdAsync(id);

            if (wedding == null)
                return NotFound();

            return Ok(wedding);
        }

        // GET /api/weddings/organizer?userId=...&isArchived=...
        [HttpGet("organizer")]
        public async Task<IActionResult> GetByOrganizer(
            [FromQuery] int userId,
            [FromQuery] bool? isArchived)
        {
            var weddings = await _weddingService.GetByOrganizerAsync(userId, isArchived);
            return Ok(weddings);
        }

        // GET /api/my
        [HttpGet("my")]
        public async Task<IActionResult> GetMyWedding()
        {
            var user = HttpContext.GetCurrentUser();

            if (user.Role != UserRole.BrideGroom)
                return Forbid();

            var wedding = await _weddingService.GetByOwnerIdAsync(user.Id);

            if (wedding == null)
                return Ok(null);

            return Ok(wedding);
        }

        // POST /api/weddings
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WeddingCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = HttpContext.GetCurrentUser();

            if (!_permissions.CanCreateWedding(user, dto.IsSelfManaged))
                return Forbid();

            var wedding = await _weddingService.CreateAsync(dto, user.Id);

            return CreatedAtAction(nameof(GetById), new { id = wedding.Id }, wedding);
        }

        // PUT /api/weddings/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] WeddingUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = HttpContext.GetCurrentUser();

            var wedding = await _weddingService.GetEntityByIdAsync(id);
            if (wedding == null)
                return NotFound();

            if (wedding.Status == WeddingStatus.Completed ||
                wedding.Status == WeddingStatus.Cancelled)
            {
                return BadRequest("Wedding cannot be modified in current state.");
            }

            if (!_permissions.CanEditWedding(user, wedding))
                return Forbid();

            await _weddingService.UpdateAsync(wedding, dto);

            return NoContent();
        }

        [HttpPut("{id}/complete")]
        public async Task<IActionResult> MarkAsCompleted(int id)
        {
            var user = HttpContext.GetCurrentUser();

            var wedding = await _weddingService.GetEntityByIdAsync(id);

            if (wedding == null)
                return NotFound();

            if (!_permissions.CanCompleteWedding(user, wedding))
                return Forbid();

            if (wedding.Status == WeddingStatus.Completed)
                return BadRequest("Wedding already completed.");

            if (wedding.Tasks == null || !wedding.Tasks.Any())
                return BadRequest("Cannot complete a wedding without tasks.");

            if (!wedding.Tasks.All(t => t.IsCompleted))
                return BadRequest("All tasks must be completed before marking the wedding as completed.");

            await _weddingService.CompleteAsync(wedding);

            return Ok();
        }

        // DELETE /api/weddings/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = HttpContext.GetCurrentUser();

            if (!_permissions.CanDeleteWedding(user))
                return Forbid();

            var wedding = await _weddingService.GetEntityByIdAsync(id);

            if (wedding == null)
                return NotFound();

            if (wedding.Tasks.Any())
                return BadRequest("Cannot delete wedding with existing tasks.");

            await _weddingService.DeleteAsync(wedding);

            return NoContent();
        }

        // PUT /api/weddings/{id}/archive
        [HttpPut("{id}/archive")]
        public async Task<IActionResult> SetArchiveState(int id, [FromBody] WeddingArchiveDto dto)
        {
            var user = HttpContext.GetCurrentUser();

            if (!_permissions.CanArchiveWedding(user))
                return Forbid();

            var wedding = await _weddingService.GetEntityByIdAsync(id);

            if (wedding == null)
                return NotFound();

            if (dto.IsArchived && wedding.Status != WeddingStatus.Completed)
            {
                return BadRequest("Only completed weddings can be archived.");
            }

            await _weddingService.SetArchiveStateAsync(wedding, dto.IsArchived);

            return NoContent();
        }
    }
}
