using trsaints_frontend_api.Entities;

namespace trsaints_frontend_api.Repositories.Interfaces;

public interface IProjectRepository : IRepository<Project>
{
    Task<IEnumerable<Project>> GetProjectsByStackAsync(int stackId);
    Task<IEnumerable<Project>> FindProjectWithStackAsync(string criteria);
}
