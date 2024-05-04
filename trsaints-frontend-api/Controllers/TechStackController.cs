using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using trsaints_frontend_api.DTOs;
using trsaints_frontend_api.Entities;
using trsaints_frontend_api.Repositories.Interfaces;

namespace trsaints_frontend_api.Controllers;

[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Bearer")]
[ApiController]
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
    public async Task<ActionResult<IEnumerable<TechStackDTO>>> GetAll()
    {
        var stacks = await _techStackRepository.GetAllAsync();
        var stacksDto = _mapper.Map<IEnumerable<TechStackDTO>>(stacks);
        
        return Ok(stacksDto);
    }

    [HttpGet("{id:int}", Name = "GetStack")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<TechStackDTO>> GetById(int id)
    {
        var stack = await _techStackRepository.GetByIdAsync(id);
        var stackDto = _mapper.Map<TechStackDTO>(stack);

        return Ok(stackDto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> Post([FromBody] TechStackDTO techStackDto)
    {
        var stack = _mapper.Map<TechStack>(techStackDto);

        await _techStackRepository.AddAsync(stack);

        return new CreatedAtRouteResult("GetStack", new { id = techStackDto.Id }, techStackDto);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Put(int id, [FromBody] TechStackDTO techStackDto)
    {
        if (id != techStackDto.Id)
            return BadRequest();
        
        var stack = _mapper.Map<TechStack>(techStackDto);
        await _techStackRepository.UpdateAsync(stack);

        return Ok(techStackDto);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Delete(int id)
    {
        var stack = await _techStackRepository.GetByIdAsync(id);
        await _techStackRepository.RemoveAsync(id);

        return Ok(stack);
    }
}
