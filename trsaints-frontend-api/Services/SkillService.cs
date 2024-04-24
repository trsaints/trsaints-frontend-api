using trsaints_frontend_api.Models;

namespace trsaints_frontend_api.Services;

public static class SkillService
{
    private static List<Skill>? Skills { get; } = [];
    private static int _nextId;
    
    public static List<Skill>? GetAll() => Skills;

    public static Skill? Get(int id) => Skills?.FirstOrDefault(s => s.Id == id);

    public static void Add(Skill skill)
    {
        skill.Id = _nextId++;
        Skills?.Add(skill);
    }

    public static void Delete(int id)
    {
        var skill = Get(id);

        if (skill is null) 
            return;

        Skills?.Remove(skill);
    }

    public static void Update(Skill skill)
    {
        if (Skills is null) 
            return;

        var index = Skills.FindIndex(s => s.Id == skill.Id);

        if (index is -1)
            return;

        Skills[index] = skill;
    }
}