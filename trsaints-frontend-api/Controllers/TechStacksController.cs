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
public class TechStacksController : DI_BaseController
{
    private readonly ITechStackRepository _techStackRepository;
    private readonly IMapper _mapper;

    public TechStacksController(
        AppDbContext context,
        IAuthorizationService authorizationService,
        UserManager<ApplicationUser> userManager,
        ITechStackRepository repository, 
        IMapper mapper): base(context, authorizationService, userManager)
    {
        _techStackRepository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize(Policy = ApiKeyDefaults.AuthenticationPolicy)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TechStackDTO>>> Get()
    {
        var stacks = await _techStackRepository.GetAllAsync();
        var stacksDto = _mapper.Map<IEnumerable<TechStackDTO>>(stacks);
        
        return Ok(stacksDto);
    }


    [HttpGet("{id:int}")]
    [Authorize(Policy = ApiKeyDefaults.AuthenticationPolicy)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TechStackDTO>> GetById(int id)
    {
        var stack = await _techStackRepository.GetByIdAsync(id);
        var stackDto = _mapper.Map<TechStackDTO>(stack);

        return Ok(stackDto);
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> Add([FromBody] TechStackDTO techStackDto)
    {
        var stack = _mapper.Map<TechStack>(techStackDto);
        var isAuthorized = await AuthorizationService.AuthorizeAsync(
            User, stack, ResourceOperations.Create);

        if (!isAuthorized.Succeeded)
            return Forbid();

        await _techStackRepository.AddAsync(stack);

        return Created($"/api/TechStack/{stack.Id}", techStackDto);
    }

    [HttpPut("{id:int}")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> Update(int id, [FromBody] TechStackDTO techStackDto)
    {
        if (id != techStackDto.Id)
            return BadRequest();
        
        var stack = _mapper.Map<TechStack>(techStackDto);
        var isAuthorized = await AuthorizationService.AuthorizeAsync(
            User, stack, ResourceOperations.Update);

        if (!isAuthorized.Succeeded)
            return Forbid();
        
        await _techStackRepository.UpdateAsync(stack);

        return Ok(techStackDto);
    }

    [HttpDelete("{id:int}")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> Remove(int id)
    {
        var stack = await _techStackRepository.GetByIdAsync(id);
        var isAuthorized = await AuthorizationService.AuthorizeAsync(
            User, stack, ResourceOperations.Delete);

        if (!isAuthorized.Succeeded)
            return Forbid();
        
        await _techStackRepository.RemoveAsync(id);

        return NoContent();
    }
}
