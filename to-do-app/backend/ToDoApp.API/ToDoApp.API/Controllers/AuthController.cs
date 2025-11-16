using Microsoft.AspNetCore.Mvc;
using ToDoApp.API.Models.Dtos;
using ToDoApp.API.Repositories.Interfaces;

namespace ToDoApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;

        public AuthController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        [HttpGet]
        [Route("login")]
        public async Task<IActionResult> GetByUsernameAndPasswordAsync([FromQuery] LoginRequest req)
        {
            var user = await _usersRepository.GetByUsernameAndPasswordAsync(req.Username, req.Password);
            if (user == null)
            {
                return BadRequest();
            }

            return Ok(user.Id);
        }
    }
}
