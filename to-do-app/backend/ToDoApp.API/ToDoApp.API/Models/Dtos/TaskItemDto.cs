namespace ToDoApp.API.Models.Dtos;

public class TaskItemDto
{
    public Guid Id { get; init; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; init; }
}
