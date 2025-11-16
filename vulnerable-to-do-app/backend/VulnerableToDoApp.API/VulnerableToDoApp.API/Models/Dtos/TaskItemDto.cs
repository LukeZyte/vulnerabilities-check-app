namespace VulnerableToDoApp.API.Models.Dtos;

public class TaskItemDto
{
    public Guid Id { get; init; }
    public string Text { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; init; }
    public Guid OwnerId { get; init; }
}
