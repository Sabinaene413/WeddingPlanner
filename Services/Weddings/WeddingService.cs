using Microsoft.EntityFrameworkCore;
using WeddingPlanner.Api.Data;
using WeddingPlanner.Api.Dtos.Weddings;
using WeddingPlanner.Api.Dtos.WeddingTasks;
using WeddingPlanner.Api.Models;
using WeddingPlanner.Api.Models.Enums;
using System.Linq.Expressions;

namespace WeddingPlanner.Api.Services.Weddings
{
    public class WeddingService : IWeddingService
    {
        private readonly WeddingPlannerContext _context;

        public WeddingService(WeddingPlannerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WeddingReadDto>> GetAllAsync(bool? archived)
        {
            var query = _context.Weddings.AsNoTracking();

            if (archived.HasValue)
            {
                query = query.Where(w => w.IsArchived == archived.Value);
            }

            return await query
                .Select(WeddingProjection())
                .ToListAsync();
        }

        public async Task<WeddingReadDto?> GetByIdAsync(int id)
        {
            return await _context.Weddings
                .AsNoTracking()
                .Where(w => w.Id == id)
                .Select(WeddingProjection())
                .FirstOrDefaultAsync();
        }

        public async Task<Wedding?> GetEntityByIdAsync(int id)
        {
            return await _context.Weddings
                .Include(w => w.Tasks)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<IEnumerable<WeddingReadDto>> GetByOrganizerAsync(int organizerId, bool? archived = null)
        {
            var query = _context.Weddings
                .AsNoTracking()
                .Where(w => w.OwnerId == organizerId);

            if (archived.HasValue)
            {
                query = query.Where(w => w.IsArchived == archived.Value);
            }

            return await query
                .Select(WeddingProjection())
                .ToListAsync();
        }

        public async Task<Wedding> CreateAsync(WeddingCreateDto dto, int ownerId)
        {
            var wedding = new Wedding
            {
                Title = dto.Title,
                Date = dto.Date,
                Location = dto.Location,
                OwnerId = ownerId,
                IsSelfManaged = dto.IsSelfManaged,
                Status = WeddingStatus.Planned,
                IsArchived = false
            };

            await _context.Weddings.AddAsync(wedding);
            await _context.SaveChangesAsync();
            return wedding;
        }

        public async Task UpdateAsync(Wedding wedding, WeddingUpdateDto dto)
        {
            wedding.Title = dto.Title;
            wedding.Date = dto.Date;
            wedding.Location = dto.Location;
            await _context.SaveChangesAsync();
        }

        public async Task CompleteAsync(Wedding wedding)
        {
            wedding.Status = WeddingStatus.Completed;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Wedding wedding)
        {
            _context.Weddings.Remove(wedding);
            await _context.SaveChangesAsync();
        }

        public async Task SetArchiveStateAsync(Wedding wedding, bool isArchived)
        {
            wedding.IsArchived = isArchived;
            await _context.SaveChangesAsync();
        }

        public async Task<Wedding?> GetByOwnerIdAsync(int ownerId)
        {
            return await _context.Weddings
                .FirstOrDefaultAsync(w => w.OwnerId == ownerId);
        }

        private static Expression<Func<Wedding, WeddingReadDto>> WeddingProjection()
        {
            return w => new WeddingReadDto
            {
                Id = w.Id,
                Title = w.Title,
                Date = w.Date,
                Location = w.Location,
                Status = w.Status.ToString(),
                IsArchived = w.IsArchived,
                OwnerId = w.OwnerId,
                Tasks = w.Tasks.Select(t => new WeddingTaskReadDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    IsCompleted = t.IsCompleted
                }).ToList()
            };
        }
    }
}
