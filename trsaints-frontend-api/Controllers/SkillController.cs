using Microsoft.AspNetCore.Mvc;
using trsaints_frontend_api.Models;
using trsaints_frontend_api.Services;

namespace trsaints_frontend_api.Controllers;

[ApiController]
[Route("[controller]")]
public class SkillController: ControllerBase
{
    [HttpGet]
    public ActionResult<List<Skill>> GetAll() => SkillService.GetAll() ?? throw new InvalidOperationException();

    [HttpGet("{id:int}")]
    public ActionResult<Skill> Get(int id)
    {
        var skill = SkillService.Get(id);

        if (skill is null)
            return NotFound();

        return skill;
    }

    [HttpPost]
    public IActionResult Create(Skill skill)
    {
        SkillService.Add(skill);
        return CreatedAtAction(nameof(Get), new { id = skill.Id }, skill);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, Skill skill)
    {
        if (id != skill.Id)
            return BadRequest();

        var currentSkill = SkillService.Get(id);

        if (currentSkill is null)
            return NotFound();
        
        SkillService.Update(skill);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var skill = SkillService.Get(id);

        if (skill is null)
            return NotFound();
        
        SkillService.Delete(id);

        return NoContent();
    }
}