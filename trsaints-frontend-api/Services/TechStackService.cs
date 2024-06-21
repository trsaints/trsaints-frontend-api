using trsaints_frontend_api.Data.Repositories.Interfaces;
using trsaints_frontend_api.Services.Interfaces;

namespace trsaints_frontend_api.Services
{
    public class TechStackService : ITechStackService
    {
        private readonly IProjectRepository _projectRepository;

        public TechStackService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public bool HasRelatedProjects(int id)
        {
            var relatedProjects = _projectRepository.GetProjectsByStackAsync(id);
            relatedProjects.Wait();

            return relatedProjects.Result.Any();
        }
    }
}