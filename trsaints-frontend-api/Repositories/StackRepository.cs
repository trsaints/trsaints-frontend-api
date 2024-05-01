using trsaints_frontend_api.Context;
using trsaints_frontend_api.Entities;
using trsaints_frontend_api.Repositories.Interfaces;

namespace trsaints_frontend_api.Repositories;

public class StackRepository: Repository<TechStack>, IStackRepository   
{
    protected StackRepository(AppDbContext db) : base(db) { }
}
