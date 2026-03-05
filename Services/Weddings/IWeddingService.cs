using WeddingPlanner.Api.Dtos.Weddings;
using WeddingPlanner.Api.Models;

namespace WeddingPlanner.Api.Services.Weddings
{
    public interface IWeddingService
    {
        Task<IEnumerable<WeddingReadDto>> GetAllAsync(bool? archived);
        Task<WeddingReadDto?> GetByIdAsync(int id);
        Task<Wedding?> GetEntityByIdAsync(int id);
        Task<IEnumerable<WeddingReadDto>> GetByOrganizerAsync(int organizerId, bool? archived = null);
        Task<Wedding> CreateAsync(WeddingCreateDto dto, int ownerId);
        Task UpdateAsync(Wedding wedding, WeddingUpdateDto dto);
        Task CompleteAsync(Wedding wedding);
        Task DeleteAsync(Wedding wedding);
        Task SetArchiveStateAsync(Wedding wedding, bool isArchived);
        Task<Wedding?> GetByOwnerIdAsync(int ownerId);
    }
}
