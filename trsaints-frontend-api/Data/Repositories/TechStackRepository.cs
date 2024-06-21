using trsaints_frontend_api.Data.Context;
using trsaints_frontend_api.Data.Entities;
using trsaints_frontend_api.Data.Repositories.Interfaces;

namespace trsaints_frontend_api.Data.Repositories;

public class TechStackRepository: Repository<TechStack>, ITechStackRepository   
{
    public TechStackRepository(AppDbContext db) : base(db) { }
}
