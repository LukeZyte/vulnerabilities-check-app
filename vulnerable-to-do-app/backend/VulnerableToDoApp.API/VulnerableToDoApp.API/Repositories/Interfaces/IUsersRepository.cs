using VulnerableToDoApp.API.Models.Domain;

namespace VulnerableToDoApp.API.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        Task<User?> GetByUsernameAndPasswordAsync(string username, string password);
    }
}
