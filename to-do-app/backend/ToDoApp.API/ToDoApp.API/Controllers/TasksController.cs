using Microsoft.AspNetCore.Mvc;
using ToDoApp.API.Models.Domain;
using ToDoApp.API.Models.Dtos;
using ToDoApp.API.Repositories.Interfaces;

namespace ToDoApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITasksRepository _tasksRepository;

    public TasksController(ITasksRepository tasksRepository)
    {
        _tasksRepository = tasksRepository;
    }

    [HttpGet]
    [Route("{taskId:Guid}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid taskId)
    {
        var task = await _tasksRepository.GetByIdAsync(taskId);
        if (task == null)
        {
            return NotFound();
        }

        return Ok(task);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var tasks = await _tasksRepository.GetAllAsync();
        if (tasks == null)
        {
            return BadRequest();
        }

        return Ok(tasks);
    }

    [HttpGet]
    [Route("owner/{ownerId:Guid}")]
    public async Task<IActionResult> GetAllByOwnerIdAsync([FromRoute] Guid ownerId)
    {
        var tasks = await _tasksRepository.GetAllByOwnerIdAsync(ownerId);
        if (tasks == null)
        {
            return BadRequest();
        }

        return Ok(tasks);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateTaskRequest request)
    {
        var task = new TaskItem
        {
            Text = request.Text,
            OwnerId = request.OwnerId
        };

        var createdTaskId = await _tasksRepository.CreateAsync(task);

        return Ok(new { taskId = createdTaskId });
    }

    [HttpDelete]
    [Route("{taskId:Guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid taskId)
    {
        var isDeleted = await _tasksRepository.DeleteByIdAsync(taskId);
        if (!isDeleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
