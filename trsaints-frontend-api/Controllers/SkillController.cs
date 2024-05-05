using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using trsaints_frontend_api.DTOs;
using trsaints_frontend_api.Entities;
using trsaints_frontend_api.Repositories;
using trsaints_frontend_api.Repositories.Interfaces;

namespace trsaints_frontend_api.Controllers;

[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Bearer")]
[ApiController]
public class SkillController: ControllerBase
{
    private readonly ISkillRepository _skillRepository;
    private readonly IMapper _mapper;

    public SkillController(ISkillRepository repository, IMapper mapper)
    {
        _skillRepository = repository;
        _mapper = mapper;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var skills = await _skillRepository.GetAllAsync();
        var skillsDto = _mapper.Map<IEnumerable<SkillDTO>>(skills);

        return Ok(skillsDto);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(int id)
    {
        var skill = await _skillRepository.GetByIdAsync(id);
        var skillDto = _mapper.Map<SkillDTO>(skill);
        
        return Ok(skillDto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    
    public async Task<IActionResult> Add(SkillDTO skillDto)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var skill = _mapper.Map<Skill>(skillDto);
        await _skillRepository.AddAsync(skill);

        return Ok(_mapper.Map<SkillDTO>(skill));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, SkillDTO skillDto)
    {
        if (id != skillDto.Id)
            return BadRequest();
        
        if (!ModelState.IsValid)
            return BadRequest();

        await _skillRepository.UpdateAsync(_mapper.Map<Skill>(skillDto));

        return Ok(skillDto);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Remove(int id)
    {
        var skill = await _skillRepository.GetByIdAsync(id);
        await _skillRepository.RemoveAsync(skill.Id);
        
        return Ok();
    }
    
    [HttpGet]
    [Route("search/{skillName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<SkillDTO>>> Search(string skillName)
    {
        var skills = await _skillRepository.SearchAsync(s => s.Name.Contains(skillName));
        var skillsDto = _mapper.Map<IEnumerable<SkillDTO>>(skills);
        
        return Ok(skillsDto);
    }
}
