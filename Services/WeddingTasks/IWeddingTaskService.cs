using WeddingPlanner.Api.Dtos.WeddingTasks;
using WeddingPlanner.Api.Models;

namespace WeddingPlanner.Api.Services.WeddingTasks
{
    public interface IWeddingTaskService
    {
        Task<IEnumerable<WeddingTaskReadDto>> GetAllAsync(bool? completed, int? weddingId);
        Task<IEnumerable<WeddingTaskReadDto>> GetByWeddingIdAsync(int weddingId);
        Task<WeddingTaskReadDto?> GetByIdAsync(int id);
        Task<WeddingTask?> GetEntityByIdAsync(int id);
        Task<WeddingTask> CreateAsync(WeddingTaskCreateDto dto);
        Task UpdateCompletionAsync(WeddingTask task, bool isCompleted);
        Task DeleteAsync(WeddingTask task);
    }
}
