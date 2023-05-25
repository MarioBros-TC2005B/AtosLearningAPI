using AtosLearningAPI.Model;
using AtosLearningAPI.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace AtosLearningAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]  
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _userRepository.GetAllUsers()); 
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            return Ok(await _userRepository.GetUserById(id)); 
        }


        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (user == null)
                return BadRequest(); 

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _userRepository.InsertUser(user);

            return Created("created", created); 
        }


        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            if (user == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return NoContent(); 
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userRepository.DeleteUser(new User { UserId = id });

            return NoContent(); 
        }
    }
}
