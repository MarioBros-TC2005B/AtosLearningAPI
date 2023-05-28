
using AtosLearningAPI.Data.Repositories;
using AtosLearningAPI.Model;
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
        
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] AuthUser user)
        {
            return Ok(await _authRepository.Login(user.username, user.password));
        }
        
    }
}
