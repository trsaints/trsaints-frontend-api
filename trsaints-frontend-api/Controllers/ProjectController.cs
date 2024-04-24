using Microsoft.AspNetCore.Mvc;
using trsaints_frontend_api.Models;
using trsaints_frontend_api.Services;

namespace trsaints_frontend_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController: ControllerBase
    {
        private readonly ILogger<ProjectController> _logger;

        public ProjectController(ILogger<ProjectController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<List<Project>> GetAll() => ProjectService.GetAll() ?? throw new InvalidOperationException();

        [HttpGet("{id:int}")]
        public ActionResult<Project> Get(int id)
        {
            var project = ProjectService.Get(id);

            if (project is null)
                return NotFound();

            return project;
        }

        [HttpPost]
        public IActionResult Create(Project project)
        {            
            _logger.LogDebug("Received POST request to create project: {Project}", project);
            ProjectService.Add(project);
            return CreatedAtAction(nameof(Get), new { id = project.Id }, project);
        }
        
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, Project project)
        {
            if (id != project.Id)
                return BadRequest();

            var currentProject = ProjectService.Get(id);

            if (currentProject is null)
                return NotFound();
            
            ProjectService.Update(project);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var project = ProjectService.Get(id);

            if (project is null)
                return NotFound();
            
            ProjectService.Delete(id);

            return NoContent();
        }
    }
}