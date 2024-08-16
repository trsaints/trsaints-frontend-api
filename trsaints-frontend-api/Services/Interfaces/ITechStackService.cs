namespace trsaints_frontend_api.Services.Interfaces;

public interface ITechStackService
{
    bool HasRelatedProjects(int id);
    Task<bool> StackExists(int id);
}
