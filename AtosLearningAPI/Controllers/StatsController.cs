using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AtosLearningAPI.Data.Repositories.QuestionStat;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AtosLearningAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly IQuestionStatRepository _statRepository;
        
        public StatsController(IQuestionStatRepository statRepository)
        {
            _statRepository = statRepository;
        }
        
        [HttpGet("{examId}")]
        public async Task<IActionResult> GetQuestionStatsByExam(int examId)
        {
            if (examId == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(await _statRepository.GetQuestionStatsByExam(examId));
        }
        
    }
}
