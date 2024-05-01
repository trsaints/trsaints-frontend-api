using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using trsaints_frontend_api.DTOs;
using trsaints_frontend_api.Entities;
using trsaints_frontend_api.Repositories.Interfaces;

namespace trsaints_frontend_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
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
        public async Task<IActionResult> GetAll()
        {
            var projects = await _projectRepository.GetAllAsync();

            return Ok(_mapper.Map<IEnumerable<ProjectDTO>>(projects));
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            
            if (project is null)
                return NotFound();

            return Ok(_mapper.Map<ProjectDTO>(project));
        }

        [HttpGet]
        [Route("get-projects-by-category/{categoryId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProjectsByStack(int stackId)
        {
            var projects = await _projectRepository.GetProjectsByStackAsync(stackId);

            if (!projects.Any())
                return NotFound();
            
            return Ok(_mapper.Map<IEnumerable<ProjectDTO>>(projects));
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add(ProjectDTO projectDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var project = _mapper.Map<Project>(projectDto);
            await _projectRepository.AddAsync(project);

            return Ok(_mapper.Map<ProjectDTO>(project));
        }
        
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, ProjectDTO projectDto)
        {
            if (id != projectDto.Id)
                return BadRequest();
            
            if (!ModelState.IsValid)
                return BadRequest();
            
            await _projectRepository.UpdateAsync(_mapper.Map<Project>(projectDto));

            return Ok(projectDto);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Remove(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            
            if (project is null)
                return NotFound();

            await _projectRepository.RemoveAsync(project.Id);

            return Ok();
        }

        [HttpGet]
        [Route("search/{projectName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ProjectDTO>>> Search(string projectName)
        {
            var projects = await _projectRepository.SearchAsync(p => p.Name.Contains(projectName));

            if (projects is null)
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<ProjectDTO>>(projects));
        }

        [HttpGet]
        [Route("search-project-with-category/{criteria}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ProjectDTO>>> SearchProjectWithStack(string criteria)
        {
            var projects = _mapper.Map<List<Project>>(await _projectRepository.FindProjectWithStackAsync(criteria));

            if (!projects.Any())
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<ProjectStackDTO>>(projects));
        }
    }
}
