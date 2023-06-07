using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AtosLearningAPI.Data.Repositories;
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
        public async Task<IActionResult> SubmitExam([FromForm] int userId, [FromForm] int[] answersIds, [FromForm] int examId, [FromForm] string score, [FromForm] DateTime startDateTime)
            {
                var floatScore = float.Parse(score);
            if (userId == null || answersIds == null || answersIds.Length == 0)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(await _examRepository.SubmitExam(userId, answersIds, examId, floatScore, startDateTime));
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

    }
}
