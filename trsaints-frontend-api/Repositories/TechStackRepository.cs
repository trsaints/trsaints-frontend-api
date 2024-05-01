using trsaints_frontend_api.Context;
using trsaints_frontend_api.Entities;
using trsaints_frontend_api.Repositories.Interfaces;

namespace trsaints_frontend_api.Repositories;

public class TechStackRepository: Repository<TechStack>, ITechStackRepository   
{
    protected TechStackRepository(AppDbContext db) : base(db) { }
}
