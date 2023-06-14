using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AtosLearning.Models;
using AtosLearningAPI.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AtosLearningAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseRepository _courseRepository;
        
        public CoursesController(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }
        
        [HttpPost]
        public async Task<IActionResult> JoinCourse([FromBody] StudentCourseDTO dto)
        {
            if (dto.CourseCode == null || dto.StudentId == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(await _courseRepository.JoinCourse(dto.StudentId, dto.CourseCode));
        }
        
    }
}
