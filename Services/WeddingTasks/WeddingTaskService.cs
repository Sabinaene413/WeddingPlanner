using Microsoft.EntityFrameworkCore;
using WeddingPlanner.Api.Data;
using WeddingPlanner.Api.Dtos.WeddingTasks;
using WeddingPlanner.Api.Models;
using WeddingPlanner.Api.Models.Enums;
using System.Linq.Expressions;

namespace WeddingPlanner.Api.Services.WeddingTasks
{
    public class WeddingTaskService : IWeddingTaskService
    {
        private readonly WeddingPlannerContext _context;

        public WeddingTaskService(WeddingPlannerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WeddingTaskReadDto>> GetAllAsync(bool? completed, int? weddingId)
        {
            var query = _context.WeddingTasks.AsNoTracking();

            if (completed.HasValue)
            {
                query = query.Where(t => t.IsCompleted == completed.Value);
            }

            if (weddingId.HasValue)
            {
                query = query.Where(t => t.WeddingId == weddingId.Value);
            }

            return await query
                .Select(TaskProjection())
                .ToListAsync();
        }

        public async Task<IEnumerable<WeddingTaskReadDto>> GetByWeddingIdAsync(int weddingId)
        {
            return await _context.WeddingTasks
                .AsNoTracking()
                .Where(t => t.WeddingId == weddingId)
                .Select(TaskProjection())
                .ToListAsync();
        }

        public async Task<WeddingTaskReadDto?> GetByIdAsync(int id)
        {
            return await _context.WeddingTasks
                .AsNoTracking()
                .Where(t => t.Id == id)
                .Select(TaskProjection())
                .FirstOrDefaultAsync();
        }

        public async Task<WeddingTask?> GetEntityByIdAsync(int id)
        {
            return await _context.WeddingTasks
                .Include(t => t.Wedding)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<WeddingTask> CreateAsync(WeddingTaskCreateDto dto)
        {
            var task = new WeddingTask
            {
                Title = dto.Title,
                WeddingId = dto.WeddingId,
                IsCompleted = false
            };

            await _context.WeddingTasks.AddAsync(task);
            await _context.SaveChangesAsync();
            await UpdateWeddingStatusAsync(task.WeddingId);
            return task;
        }

        public async Task UpdateCompletionAsync(WeddingTask task, bool isCompleted)
        {
            task.IsCompleted = isCompleted;
            await _context.SaveChangesAsync();
            await UpdateWeddingStatusAsync(task.WeddingId);
        }

        public async Task DeleteAsync(WeddingTask task)
        {
            var weddingId = task.WeddingId;
            _context.WeddingTasks.Remove(task);
            await _context.SaveChangesAsync();
            await UpdateWeddingStatusAsync(weddingId);
        }

        private async Task UpdateWeddingStatusAsync(int weddingId)
        {
            var wedding = await _context.Weddings
                .Include(w => w.Tasks)
                .FirstOrDefaultAsync(w => w.Id == weddingId);

            if (wedding == null) return;

            // Logic to update status based on tasks
            if (!wedding.Tasks.Any())
            {
                wedding.Status = WeddingStatus.Planned;
            }
            else if (wedding.Status != WeddingStatus.Completed && wedding.Status != WeddingStatus.Cancelled)
            {
                wedding.Status = WeddingStatus.InProgress;
            }

            await _context.SaveChangesAsync();
        }

        private static Expression<Func<WeddingTask, WeddingTaskReadDto>> TaskProjection()
        {
            return t => new WeddingTaskReadDto
            {
                Id = t.Id,
                Title = t.Title,
                IsCompleted = t.IsCompleted
            };
        }
    }
}
