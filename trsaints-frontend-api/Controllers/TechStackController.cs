using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using trsaints_frontend_api.DTOs;
using trsaints_frontend_api.Entities;
using trsaints_frontend_api.Repositories.Interfaces;

namespace trsaints_frontend_api.Controllers;

public class TechStackController : ControllerBase
{
    private readonly IStackRepository _stackRepository;
    private readonly IMapper _mapper;

    public TechStackController(IStackRepository repository, IMapper mapper)
    {
        _stackRepository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TechStackDTO>>> GetAll()
    {
        var stacks = await _stackRepository.GetAllAsync();
        var stacksDto = _mapper.Map<IEnumerable<TechStackDTO>>(stacks);
        
        return Ok(stacksDto);
    }

    [HttpGet("{id:int}", Name = "GetStack")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<TechStackDTO>> GetById(int id)
    {
        var stack = await _stackRepository.GetByIdAsync(id);
        var stackDto = _mapper.Map<TechStackDTO>(stack);

        return Ok(stackDto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> Post([FromBody] TechStackDTO techStackDto)
    {
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
        
        var stack = _mapper.Map<TechStack>(techStackDto);
        await _stackRepository.UpdateAsync(stack);

        return Ok(techStackDto);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Delete(int id)
    {
        var stack = await _stackRepository.GetByIdAsync(id);
        await _stackRepository.RemoveAsync(id);

        return Ok(stack);
    }
}
