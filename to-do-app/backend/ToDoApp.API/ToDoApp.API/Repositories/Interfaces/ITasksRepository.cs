using ToDoApp.API.Models.Domain;

namespace ToDoApp.API.Repositories.Interfaces;

public interface ITasksRepository
{
    Task<TaskItem?> GetByIdAsync(Guid taskId);
    Task<IList<TaskItem>> GetAllAsync();
    Task<bool> DeleteByIdAsync(Guid taskId);
    Task<Guid> CreateAsync(TaskItem taskItem);
    Task<bool> UpdateAsync(TaskItem taskItem);
}
