using trsaints_frontend_api.Data.Entities;

namespace trsaints_frontend_api.Data.Repositories.Interfaces;

public interface IRelatedProjectsRepository
{
    Task<IEnumerable<Project>> GetProjectsByStackAsync(int techStackId);
}