namespace trsaints_frontend_api.Data.Entities;

public class TechStackSkill : Entity
{
    public int SkillId { get; set; }
    public int TechStackId { get; set; }
    public Skill Skill { get; set; }
    public TechStack TechStack { get; set; }
}
