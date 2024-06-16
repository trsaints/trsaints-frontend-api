using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using trsaints_frontend_api.Authorization;
using trsaints_frontend_api.DTOs;
using trsaints_frontend_api.Entities;
using trsaints_frontend_api.Repositories.Interfaces;

namespace trsaints_frontend_api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Bearer")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class ProjectController: ControllerBase
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;

    public ProjectController(IProjectRepository repository, IMapper mapper)
    {
        _projectRepository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var projects = await _projectRepository.GetAllAsync();
        var projectDto = _mapper.Map<IEnumerable<ProjectDTO>>(projects);
            
        return Ok(projectDto);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        var projectDto = _mapper.Map<ProjectDTO>(project);
            
        return Ok(projectDto);
    }

    [HttpGet]
    [Route("stack/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByStack(int id)
    {
        var projects = await _projectRepository.GetProjectsByStackAsync(id);

        if (!projects.Any())
            return NotFound();
            
        return Ok(_mapper.Map<IEnumerable<ProjectDTO>>(projects));
    }
        
    [HttpPost]
    [Authorize(Roles = ResourceOperationConstants.RoleAdministrators)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Add(ProjectDTO projectDto)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var project = _mapper.Map<Project>(projectDto);
        await _projectRepository.AddAsync(project);

        return Ok(_mapper.Map<ProjectDTO>(project));
    }
        
    [HttpPut("{id:int}")]
    [Authorize(Roles = ResourceOperationConstants.RoleAdministrators)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Update(int id, ProjectDTO projectDto)
    {
        if (id != projectDto.Id || !ModelState.IsValid)
            return BadRequest();
            
        await _projectRepository.UpdateAsync(_mapper.Map<Project>(projectDto));

        return Ok(projectDto);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = ResourceOperationConstants.RoleAdministrators)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Remove(int id)
    {
        var project = await _projectRepository.GetByIdAsync(id);

        await _projectRepository.RemoveAsync(project.Id);

        return Ok();
    }

    [HttpGet]
    [Route("name/{projectName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ProjectDTO>>> SearchByName(string projectName)
    {
        var projects = await _projectRepository.SearchAsync(p => p.Name.Contains(projectName));

        return Ok(_mapper.Map<IEnumerable<ProjectDTO>>(projects));
    }

    [HttpGet]
    [Route("stack/{criteria}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ProjectDTO>>> SearchByStack(string criteria)
    {
        var projects = _mapper.Map<List<Project>>(await _projectRepository.FindProjectWithStackAsync(criteria));

        return Ok(_mapper.Map<IEnumerable<ProjectStackDTO>>(projects));
    }
}
