namespace trsaints_frontend_api.Data.Entities;

public class Skill : Entity
{
    public string Name { get; set; }
    public SkillCategory SkillCategory { get; set; }
    public IEnumerable<TechStack> TechStacks { get; }
    public IEnumerable<TechStackSkill> TechStackSkills { get; }
}

public enum SkillCategory
{
    HardSkill, SoftSkill
}