using trsaints_frontend_api.Models;

namespace trsaints_frontend_api.Services;

public static class ProjectService
{
    private static List<Project>? Projects { get; } = [];
    private static int _nextId;

    public static List<Project>? GetAll() => Projects;

    public static Project? Get(int id) => Projects?.FirstOrDefault(p => p.Id == id);
    
    public static void Add(Project project)
    {
        project.Id = _nextId++;
        Projects?.Add(project);
    }

    public static void Delete(int id)
    {
        var project = Get(id);

        if (project is null)
            return;

        Projects?.Remove(project);
    }

    public static void Update(Project project)
    {
        if (Projects is null) return;
        
        var index = Projects.FindIndex(p => p.Id == project.Id);

        if (index is -1)
            return;

        Projects[index] = project;
    }
}