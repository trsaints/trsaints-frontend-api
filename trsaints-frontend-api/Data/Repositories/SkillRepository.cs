using trsaints_frontend_api.Data.Context;
using trsaints_frontend_api.Data.Entities;
using trsaints_frontend_api.Data.Repositories.Interfaces;

namespace trsaints_frontend_api.Data.Repositories;

public class SkillRepository : Repository<Skill>, ISkillRepository
{
    public SkillRepository(AppDbContext db) : base(db) { }
}
