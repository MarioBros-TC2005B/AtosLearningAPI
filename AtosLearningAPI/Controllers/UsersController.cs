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
        private readonly IStudentRepository _studentRepository;

        public UsersController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        [HttpGet]  
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _studentRepository.GetAllUsers()); 
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            return Ok(await _studentRepository.GetUserById(id)); 
        }


        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] Student student)
        {
            if (student == null)
                return BadRequest(); 

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _studentRepository.InsertUser(student);

            return Created("created", created); 
        }


        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] Student student)
        {
            if (student == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _studentRepository.UpdateUser(student);
            
            return Ok(updated);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _studentRepository.DeleteUser(new Student { Id = id });

            return NoContent(); 
        }
    }
}
