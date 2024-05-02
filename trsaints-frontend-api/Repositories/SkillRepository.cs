using trsaints_frontend_api.Context;
using trsaints_frontend_api.Entities;
using trsaints_frontend_api.Repositories.Interfaces;

namespace trsaints_frontend_api.Repositories;

public class SkillRepository: Repository<Skill>, ISkillRepository
{
    public SkillRepository(AppDbContext db) : base(db) { }
}
