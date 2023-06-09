using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AtosLearningAPI.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AtosLearningAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VideoGameExamsController : ControllerBase
    {
        private readonly IVideoGameExamRepository _videoGameExamRepository;
        
        public VideoGameExamsController(IVideoGameExamRepository videoGameExamRepository)
        {
            _videoGameExamRepository = videoGameExamRepository;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetUserExams(int userId)
        {
            return Ok(await _videoGameExamRepository.GetUserExams(userId));
        }
        
        [HttpGet]
        [Route("questions")]
        public async Task<IActionResult> GetExamQuestions(int examId)
        {
            return Ok(await _videoGameExamRepository.GetExamQuestions(examId));
        }
        
        [HttpGet]
        [Route("submitted")]
        public async Task<IActionResult> GetSubmittedExams(int userId)
        {
            return Ok(await _videoGameExamRepository.GetSubmittedExams(userId));
        }
        
        [HttpGet]
        [Route("pending")]
        public async Task<IActionResult> GetPendingExams(int userId)
        {
            return Ok(await _videoGameExamRepository.GetPendingExams(userId));
        }
    }
}
