using VulnerableToDoApp.API.Models.Domain;

namespace VulnerableToDoApp.API.Repositories.Interfaces;

public interface ITasksRepository
{
    Task<TaskItem?> GetByIdAsync(Guid taskId);
    Task<IList<TaskItem>> GetAllAsync();
    Task<IList<TaskItem>> GetAllByOwnerIdAsync(Guid ownerId);
    Task<bool> DeleteByIdAsync(Guid taskId);
    Task<Guid> CreateAsync(TaskItem taskItem);
    Task<bool> UpdateAsync(TaskItem taskItem);
}
