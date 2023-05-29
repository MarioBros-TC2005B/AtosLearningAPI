using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using AtosLearningAPI.Data.Repositories;
using AtosLearningAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace AtosLearningAPI.Controllers;


[Route("[controller]")]
[ApiController]
public class SubjectsController : ControllerBase
{
    private readonly ISubjectRepository _subjectRepository; 
    
    public SubjectsController(ISubjectRepository subjectRepository)
    {
        _subjectRepository = subjectRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSubjects()
    {
        return Ok(await _subjectRepository.GetAllSubjects());
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSubjectById(int id)
    {
        return Ok(await _subjectRepository.GetSubjectById(id));
    }
    
    [HttpPost]
    public async Task<IActionResult> AddSubject([FromBody] Subject subject)
    {
        if (subject == null)
            return BadRequest();
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var created = await _subjectRepository.AddSubject(subject);

        return Created("created", created);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateSubject([FromBody] Subject subject)
    {
        if (subject == null)
            return BadRequest();
        if (!ModelState.IsValid)
            return BadRequest(ModelState); 
        await _subjectRepository.UpdateSubject(subject);

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSubject(int id)
    {
        await _subjectRepository.DeleteSubject(id);

        return NoContent();
    }
    
    [HttpGet("teacher/{teacherId}")]
    public async Task<IActionResult> GetTeacherSubjects(string teacherId)
    {
        return Ok(await _subjectRepository.GetTeacherSubjects(teacherId));
    }
}
