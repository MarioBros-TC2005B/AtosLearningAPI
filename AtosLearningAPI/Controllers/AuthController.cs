
using AtosLearningAPI.Data.Repositories;
using AtosLearningAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace AtosLearningAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
        
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] String username, [FromForm] String password)
        {
            return Ok(await _authRepository.Login(username, password));
        }
        
        
        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromForm] string name,
            [FromForm] string surname,
            [FromForm] string email,
            [FromForm] string password,
            [FromForm] bool isTeacher
        )
        {
            if (name == null || surname == null || email == null || password == null || isTeacher == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = new User()
            {
                Id = 0,
                Name = name,
                Surname = surname,
                Email = email,
                IsTeacher = isTeacher,
                TotalScore = 0,
                Course = null,
                Nickname = email
            };
            var created = await _authRepository.Register(user, password);

            return Created("created", created);
        }

    }
}
