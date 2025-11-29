using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.API.Models.Domain;
using ToDoApp.API.Models.Dtos;
using ToDoApp.API.Repositories.Interfaces;

namespace ToDoApp.API.Controllers;

[ApiController]
[Authorize]
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
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var task = await _tasksRepository.GetByIdAsync(taskId);
        if (task == null) return NotFound();

        if (task.OwnerId != Guid.Parse(userId)) return Forbid();

        return Ok(task);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var tasks = await _tasksRepository.GetAllByOwnerIdAsync(Guid.Parse(userId));
        return Ok(tasks);
    }

    [HttpGet]
    [Route("owner/{ownerId:Guid}")]
    public async Task<IActionResult> GetAllByOwnerIdAsync([FromRoute] Guid ownerId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var tasks = await _tasksRepository.GetAllByOwnerIdAsync(Guid.Parse(userId));
        return Ok(tasks);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateTaskRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        var task = new TaskItem
        {
            Text = request.Text,
            OwnerId = Guid.Parse(userId)
        };

        var createdTaskId = await _tasksRepository.CreateAsync(task);

        return Ok(new { taskId = createdTaskId });
    }

    [HttpDelete]
    [Route("{taskId:Guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid taskId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var task = await _tasksRepository.GetByIdAsync(taskId);
        if (task == null) return NotFound();

        if (task.OwnerId != Guid.Parse(userId))
            return Forbid();

        var isDeleted = await _tasksRepository.DeleteByIdAsync(taskId);
        return isDeleted ? NoContent() : BadRequest();
    }
}
