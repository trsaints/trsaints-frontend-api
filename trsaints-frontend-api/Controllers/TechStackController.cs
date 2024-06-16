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
public class TechStackController : ControllerBase
{
    private readonly ITechStackRepository _techStackRepository;
    private readonly IMapper _mapper;

    public TechStackController(ITechStackRepository repository, IMapper mapper)
    {
        _techStackRepository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TechStackDTO>>> Get()
    {
        var stacks = await _techStackRepository.GetAllAsync();
        var stacksDto = _mapper.Map<IEnumerable<TechStackDTO>>(stacks);
        
        return Ok(stacksDto);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TechStackDTO>> GetById(int id)
    {
        var stack = await _techStackRepository.GetByIdAsync(id);
        var stackDto = _mapper.Map<TechStackDTO>(stack);

        return Ok(stackDto);
    }

    [HttpPost]
    [Authorize(Roles = ResourceOperationConstants.RoleAdministrators)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> Add([FromBody] TechStackDTO techStackDto)
    {
        var stack = _mapper.Map<TechStack>(techStackDto);

        await _techStackRepository.AddAsync(stack);

        return new CreatedAtRouteResult(new { id = techStackDto.Id }, techStackDto);
    }

    [HttpPut]
    [Authorize(Roles = ResourceOperationConstants.RoleAdministrators)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> Update(int id, [FromBody] TechStackDTO techStackDto)
    {
        if (id != techStackDto.Id)
            return BadRequest();
        
        var stack = _mapper.Map<TechStack>(techStackDto);
        await _techStackRepository.UpdateAsync(stack);

        return Ok(techStackDto);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = ResourceOperationConstants.RoleAdministrators)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> Remove(int id)
    {
        var stack = await _techStackRepository.GetByIdAsync(id);
        await _techStackRepository.RemoveAsync(id);

        return Ok(stack);
    }
}
