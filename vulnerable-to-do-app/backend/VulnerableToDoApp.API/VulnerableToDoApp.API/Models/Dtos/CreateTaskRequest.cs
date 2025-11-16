namespace VulnerableToDoApp.API.Models.Dtos;

public class CreateTaskRequest
{
    public string Text { get; init; }
    public Guid OwnerId { get; init; }
}
