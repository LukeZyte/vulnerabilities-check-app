namespace ToDoApp.API.Models.Domain
{
    public class User
    {
        public Guid Id { get; init; }
        public string Username { get; init; }
        public string Password { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
