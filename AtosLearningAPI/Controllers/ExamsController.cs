using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AtosLearningAPI.Data.Repositories;
using AtosLearningAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AtosLearningAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamsController : ControllerBase
    {
        private readonly IExamRepository _examRepository;
        
        public ExamsController(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }
        
        [HttpPost]
        public async Task<IActionResult> SubmitExam([FromForm] int userId, [FromForm] int[] answersIds, [FromForm] int examId, [FromForm] string score, [FromForm] DateTime endDateTime)
            {
                var floatScore = float.Parse(score);
            if (userId == null || answersIds == null || answersIds.Length == 0)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(await _examRepository.SubmitExam(userId, answersIds, examId, floatScore, endDateTime));
        }
        
        [HttpDelete]
        public async Task<IActionResult> DeleteExamSubmission([FromForm] int userId, [FromForm] int examId)
        {
            if (userId == null || examId == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(await _examRepository.DeleteExamSubmission(userId, examId));
        }
        
        [HttpGet]
        public async Task<IActionResult> GetExams()
        {
            return Ok(await _examRepository.GetExams());
        }
        
        [HttpGet("{id}")]
        public async Task<Exam> GetExam(int id)
        {
            return await _examRepository.GetExamById(id);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddExam([FromBody] Exam exam)
        {
            if (exam == null || exam.Questions.Length == 0)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var created = await _examRepository.AddExam(exam);

            return Created("created", created);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExam(int id)
        {
            if (id == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(await _examRepository.DeleteExam(id));
        }
        
        [HttpGet("subject/{subjectId}")]
        public async Task<IActionResult> GetExamsBySubject(int subjectId)
        {
            if (subjectId == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(await _examRepository.GetExamsBySubject(subjectId));
        }
        
        [HttpGet("submitted/{studentId}")]
        public async Task<IActionResult> GetSubmittedExamsByStudent(int studentId)
        {
            if (studentId == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(await _examRepository.GetSubmittedExamsByStudent(studentId));
        }
        
        [HttpGet("pending/{studentId}")]
        public async Task<IActionResult> GetPendingExamsByStudent(int studentId)
        {
            if (studentId == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(await _examRepository.GetPendingExamsByStudent(studentId));
        }

        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetExamsByCourse(int courseId)
        {
            if (courseId == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(await _examRepository.GetExamsByCourse(courseId));
        }
        
        [HttpGet("active/{courseId}")]
        public async Task<IActionResult> GetActiveExamsByCourse(int courseId)
        {
            if (courseId == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(await _examRepository.GetActiveExamsByCourse(courseId));
        }
        
        [HttpGet("inactive/{courseId}")]
        public async Task<IActionResult> GetInactiveExamsByCourse(int courseId)
        {
            if (courseId == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(await _examRepository.GetInactiveExamsByCourse(courseId));
        }

    }
}
