using Microsoft.EntityFrameworkCore;
using trsaints_frontend_api.Context;
using trsaints_frontend_api.Entities;
using trsaints_frontend_api.Repositories.Interfaces;

namespace trsaints_frontend_api.Repositories;

public class ProjectRepository: Repository<Project>, IProjectRepository
{
    public ProjectRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Project>> GetProjectsByStackAsync(int stackId)
    {
        return await _db.Projects.Where(p => p.StackId == stackId).ToListAsync();
    }

    public async Task<IEnumerable<Project>> FindProjectWithStackAsync(string criteria)
    {
        return await _db.Projects.AsNoTracking()
            .Include(p => p.Stack)
            .Where(p => p.Name.Contains(criteria) ||
                        p.Description.Contains(criteria) ||
                        p.Stack.Name.Contains(criteria)).ToListAsync();
    }
}