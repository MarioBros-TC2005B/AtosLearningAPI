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
    public class ExamSubmissionController : ControllerBase
    {
        private readonly IExamSubmissionRepository _submissionRepository;
        
        public ExamSubmissionController(IExamSubmissionRepository submissionRepository)
        {
            _submissionRepository = submissionRepository;
        }
        
        [HttpGet("exam/{examId}")]
        public async Task<IActionResult> GetSubmissionsByExam(int examId)
        {
            return Ok(await _submissionRepository.GetSubmissionsByExam(examId));
        }
    }
}
