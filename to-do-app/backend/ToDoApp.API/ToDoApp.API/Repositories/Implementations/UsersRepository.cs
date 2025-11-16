using Dapper;
using System.Data;
using ToDoApp.API.Models.Domain;
using ToDoApp.API.Repositories.Interfaces;

namespace ToDoApp.API.Repositories.Implementations
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
            var query = @"
            SELECT Id, Username, Password
            FROM Users 
            WHERE Username = @Username AND Password = @Password"
            ;

            var user = await _connection.QueryFirstOrDefaultAsync<User>(query, new {Username = username, Password = password});

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
