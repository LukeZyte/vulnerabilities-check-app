using ToDoApp.API.Models.Domain;

namespace ToDoApp.API.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        Task<User?> GetByUsernameAndPasswordAsync(string username, string password);
    }
}
