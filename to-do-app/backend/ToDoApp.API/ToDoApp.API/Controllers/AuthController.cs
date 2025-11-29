using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.API.Models.Dtos;
using ToDoApp.API.Repositories.Interfaces;
using ToDoApp.API.Services;

namespace ToDoApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IJwtService _jwtService;

        public AuthController(
            IUsersRepository usersRepository,
            IJwtService jwtService)
        {
            _usersRepository = usersRepository;
            _jwtService = jwtService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var user = await _usersRepository
                .GetByUsernameAndPasswordAsync(req.Username, req.Password);

            if (user == null)
            {
                return Unauthorized("Invalid credentials");
            }

            var token = _jwtService.GenerateToken(user.Id, user.Username);

            return Ok(new
            {
                token = token,
                userId = user.Id.ToString()
            });
        }
    }
}
