namespace ToDoApp.API.Models.Domain;

public class TaskItem
{
    public Guid Id { get; init; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; init; }
}
