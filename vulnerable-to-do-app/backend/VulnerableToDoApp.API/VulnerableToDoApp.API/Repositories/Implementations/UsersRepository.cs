using Dapper;
using System.Data;
using VulnerableToDoApp.API.Models.Domain;
using VulnerableToDoApp.API.Repositories.Interfaces;

namespace VulnerableToDoApp.API.Repositories.Implementations
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ILogger<UsersRepository> _logger;
        private readonly IDbConnection _connection;

        public UsersRepository(ILogger<UsersRepository> logger, IDbConnection connection)
        {
            _logger = logger;
            _connection = connection;
        }

        public async Task<User?> GetByUsernameAndPasswordAsync(string username, string password)
        {
            var query = $"SELECT Id, Username, Password FROM Users WHERE Username = '{username}' AND Password = '{password}'";

            var user = await _connection.QueryFirstOrDefaultAsync<User>(query);

            if (user == null)
            {
                _logger.LogInformation($"User not found by username: {username}");
                return null;
            }
            else
            {
                _logger.LogInformation($"User found with username: {user.Username}");
            }

            return user;
        }
    }
}
