using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using trsaints_frontend_api.Authorization;
using trsaints_frontend_api.Constants;
using trsaints_frontend_api.Data.Context;
using trsaints_frontend_api.Data.DTOs;
using trsaints_frontend_api.Data.Entities;
using trsaints_frontend_api.Data.Repositories.Interfaces;

namespace trsaints_frontend_api.Controllers;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class SkillsController: DI_BaseController
{
    private readonly ISkillRepository _skillRepository;
    private readonly IMapper _mapper;

    public SkillsController(
        AppDbContext context,
        IAuthorizationService authorizationService,
        UserManager<ApplicationUser> userManager,
        ISkillRepository skillRepository,
        IMapper mapper)
        : base(context, authorizationService, userManager)
    {
        _skillRepository = skillRepository;
        _mapper = mapper;
    }
    
    [HttpGet]
    [Authorize(Policy = ApiKeyDefaults.AuthenticationPolicy)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var skills = await _skillRepository.GetAllAsync();
        var skillsDto = _mapper.Map<IEnumerable<SkillDTO>>(skills);

        return Ok(skillsDto);
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = ApiKeyDefaults.AuthenticationPolicy)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var skill = await _skillRepository.GetByIdAsync(id);
        var skillDto = _mapper.Map<SkillDTO>(skill);
        
        return Ok(skillDto);
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Add(SkillDTO skillDto)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var skill = _mapper.Map<Skill>(skillDto);
        var isAuthorized = await AuthorizationService.AuthorizeAsync(
            User, skill, ResourceOperations.Create);

        if (!isAuthorized.Succeeded)
            return Forbid();
        
        await _skillRepository.AddAsync(skill);

        return Created($"/api/Skills/{skill.Id}", skillDto);
    }

    [HttpPut("{id:int}")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Update(int id, SkillDTO skillDto)
    {
        if (id != skillDto.Id)
            return BadRequest();
        
        if (!ModelState.IsValid)
            return BadRequest();
        
        var skill = _mapper.Map<Skill>(skillDto);
        var isAuthorized = await AuthorizationService.AuthorizeAsync(
            User, skill, ResourceOperations.Update);

        if (!isAuthorized.Succeeded)
            return Forbid();

        await _skillRepository.UpdateAsync(_mapper.Map<Skill>(skillDto));

        return Ok(skillDto);
    }

    [HttpDelete("{id:int}")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Remove(int id)
    {
        var skill = await _skillRepository.GetByIdAsync(id);
        var isAuthorized = await AuthorizationService.AuthorizeAsync(
            User, skill, ResourceOperations.Delete);

        if (!isAuthorized.Succeeded)
            return Forbid();
        
        await _skillRepository.RemoveAsync(skill.Id);
        
        return Ok();
    }
    
    [HttpGet("search/{name}")]
    [Authorize(Policy = ApiKeyDefaults.AuthenticationPolicy)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<SkillDTO>>> Search(string name)
    {
        var skills = await _skillRepository.SearchAsync(s => s.Name.Contains(name));
        var skillsDto = _mapper.Map<IEnumerable<SkillDTO>>(skills);
        
        return Ok(skillsDto);
    }
}
