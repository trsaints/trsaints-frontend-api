using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using trsaints_frontend_api.Data.Context;
using trsaints_frontend_api.Data.Entities;
using trsaints_frontend_api.Data.Repositories.Interfaces;

namespace trsaints_frontend_api.Data.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
{
    protected readonly AppDbContext Db;
    private readonly DbSet<TEntity> _dbSet;

    protected Repository(AppDbContext db)
    {
        Db = db;
        _dbSet = Db.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
    }

    public async Task<TEntity> GetByIdAsync(int? id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<List<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task AddAsync(TEntity entity)
    {
        _dbSet.Add(entity);
        await Db.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        await Db.SaveChangesAsync();
    }

    public async Task RemoveAsync(int? id)
    {
        var entity = await _dbSet.FindAsync(id);
        _dbSet.Remove(entity);
        await Db.SaveChangesAsync();
    }

    public void Dispose()
    {
        Db?.Dispose();
    }
}
