using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using trsaints_frontend_api.DTOs;
using trsaints_frontend_api.Entities;
using trsaints_frontend_api.Repositories.Interfaces;

namespace trsaints_frontend_api.Controllers;

public class StackController : ControllerBase
{
    private readonly IStackRepository _stackRepository;
    private readonly IMapper _mapper;

    public StackController(IStackRepository repository, IMapper mapper)
    {
        _stackRepository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<TechStackDTO>>> Get()
    {
        var stacks = await _stackRepository.GetAllAsync();

        if (stacks is null)
            return NotFound();

        var stacksDto = _mapper.Map<IEnumerable<TechStackDTO>>(stacks);
        return Ok(stacksDto);
    }

    [HttpGet("{id:int}", Name = "GetStack")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TechStackDTO>> Get(int id)
    {
        var stack = await _stackRepository.GetByIdAsync(id);

        if (stack is null)
            return NotFound();

        var stackDto = _mapper.Map<TechStackDTO>(stack);

        return Ok(stackDto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Post([FromBody] TechStackDTO techStackDto)
    {
        if (techStackDto is null)
            return BadRequest();

        var stack = _mapper.Map<TechStack>(techStackDto);

        await _stackRepository.AddAsync(stack);

        return new CreatedAtRouteResult("GetStack", new { id = techStackDto.Id }, techStackDto);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Put(int id, [FromBody] TechStackDTO techStackDto)
    {
        if (id != techStackDto.Id)
            return BadRequest();

        if (techStackDto is null)
            return BadRequest();

        var stack = _mapper.Map<TechStack>(techStackDto);
        await _stackRepository.UpdateAsync(stack);

        return Ok(techStackDto);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var stack = await _stackRepository.GetByIdAsync(id);

        if (stack is null)
            return NotFound();

        await _stackRepository.RemoveAsync(id);

        return Ok(stack);
    }
}
