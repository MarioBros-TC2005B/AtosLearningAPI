
using AtosLearningAPI.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AtosLearningAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            return Ok(await _authRepository.Login(username, password));
        }
        
    }
}
