﻿using AtosLearningAPI.Model;
using AtosLearningAPI.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace AtosLearningAPI.Controllers
{
    [Route("[controller]")]
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

            var updated = await _userRepository.UpdateUser(user);
            
            return Ok(updated);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userRepository.DeleteUser(new User { Id = id });

            return NoContent(); 
        }
        
        [HttpPut]
        [Route("updateNickname")]
        public async Task<IActionResult> UpdateNickname([FromForm] int userId, [FromForm] string nickname)
        {
            if (userId == 0 || nickname == null)
                return BadRequest();

            var updated = await _userRepository.UpdateNickname(userId, nickname);
            return Ok(updated);
        }
    }
}
