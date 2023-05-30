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
        public async Task<IActionResult> GetUserExams(string userId)
        {
            return Ok(await _videoGameExamRepository.GetUserExams(userId));
        }
    }
}
