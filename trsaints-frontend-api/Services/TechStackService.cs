using trsaints_frontend_api.Data.Repositories.Interfaces;
using trsaints_frontend_api.Services.Interfaces;

namespace trsaints_frontend_api.Services
{
    public class TechStackService : ITechStackService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITechStackRepository _techStackRepository;

        public TechStackService(IProjectRepository projectRepository, ITechStackRepository techStackRepository)
        {
            _projectRepository = projectRepository;
            _techStackRepository = techStackRepository;
        }

        public bool HasRelatedProjects(int id)
        {
            var relatedProjects = _projectRepository.GetProjectsByStackAsync(id);
            relatedProjects.Wait();

            return relatedProjects.Result.Any();
        }

        public async Task<bool> StackExists(int id)
        {
            var stack = await _techStackRepository.GetByIdAsync(id);
            return stack != null;
        }
    }
}