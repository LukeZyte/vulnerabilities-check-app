using Dapper;
using System.Data;
using ToDoApp.API.Models.Domain;
using ToDoApp.API.Repositories.Interfaces;

namespace ToDoApp.API.Repositories.Implementations;

public class TasksRepository : ITasksRepository
{
    private readonly ILogger<TasksRepository> _logger;
    private readonly IDbConnection _connection;

    public TasksRepository(ILogger<TasksRepository> logger, IDbConnection connection)
    {
        _logger = logger;
        _connection = connection;
    }

    public async Task<TaskItem?> GetByIdAsync(Guid taskId)
    {
        var query = @"
            SELECT Id, Title, Description, IsCompleted, CreatedAt 
            FROM Tasks 
            WHERE Id = @Id";

        var taskItem = await _connection.QueryFirstOrDefaultAsync<TaskItem>(query, new { Id = taskId });

        if (taskItem == null)
        {
            _logger.LogInformation($"Task not found by Id: {taskId}");
            return null;
        }
        else
        {
            _logger.LogInformation($"Task found with title: {taskItem.Title}");
        }

        return taskItem;
    }

    public async Task<bool> DeleteByIdAsync(Guid taskId)
    {
        var query = "DELETE FROM Tasks WHERE Id = @Id";

        var rowsAffected = await _connection.ExecuteAsync(query, new { Id = taskId });

        if (rowsAffected == 0)
        {
            _logger.LogWarning($"Task not found for deletion. Id: {taskId}");
            return false;
        }

        _logger.LogInformation($"Task deleted successfully. Id: {taskId}");
        return true;
    }

    public async Task<Guid> CreateAsync(TaskItem taskItem)
    {
        var query = @"
        INSERT INTO Tasks (Title, Description) 
        OUTPUT INSERTED.Id
        VALUES (@Title, @Description)";

        var newId = await _connection.QuerySingleAsync<Guid>(query, new
        {
            taskItem.Title,
            taskItem.Description,
        });

        _logger.LogInformation($"Task created successfully. Id: {newId}, Title: {taskItem.Title}");

        return newId;
    }

    public async Task<bool> UpdateAsync(TaskItem taskItem)
    {
        var query = @"
        UPDATE Tasks 
        SET 
            Title = @Title,
            Description = @Description,
            IsCompleted = @IsCompleted,
        END
        WHERE Id = @Id";

        var rowsAffected = await _connection.ExecuteAsync(query, new
        {
            taskItem.Id,
            taskItem.Title,
            taskItem.Description,
            taskItem.IsCompleted
        });

        if (rowsAffected == 0)
        {
            _logger.LogWarning($"Task not found for update. Id: {taskItem.Id}");
            return false;
        }

        _logger.LogInformation($"Task updated successfully. Id: {taskItem.Id}, Title: {taskItem.Title}");
        return true;
    }

    public async Task<IList<TaskItem>> GetAllAsync()
    {
        var query = @"
            SELECT Id, Title, Description, IsCompleted, CreatedAt 
            FROM Tasks
            ORDER BY CreatedAt DESC"
        ;

        var taskItems = await _connection.QueryAsync<TaskItem>(query);

        return taskItems.ToList();
    }
}
