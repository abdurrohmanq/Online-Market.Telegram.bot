using Microsoft.EntityFrameworkCore;
using OnlineMarket.Data.DbContexts;
using OnlineMarket.Data.IRepositories;
using OnlineMarket.Domain.Commons;
using System.Linq.Expressions;

namespace OnlineMarket.Data.Repositories;

public class Repository<T> : IRepository<T> where T : Auditable
{
    private readonly DbSet<T> dbSet;

    private readonly AppDbContext appDbContext;

    public Repository(AppDbContext appDbContext)
    {
        dbSet = appDbContext.Set<T>();
        this.appDbContext = appDbContext;
    }

    public async Task CreateAsync(T entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        appDbContext.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(T entity)
    {
        entity.IsDelete = true;
        appDbContext.Entry(entity).State = EntityState.Modified;
    }

    public void Destroy(T entity)
    {
        dbSet.Remove(entity);
    }

    public void Destroy(Expression<Func<T, bool>> expression)
    {
        var entities = dbSet.Where(expression);
        if(entities is not null)
        {
            foreach(var entity in entities)
            {
                dbSet.Remove(entity);
            }
        }
    }

    public IQueryable<T> GetAll(Expression<Func<T, bool>> expression = null, bool isNoTracked = true, string[] includes = null)
    {
        IQueryable<T> queryable;
        if (expression != null)
        {
            queryable = dbSet.Where(expression).AsQueryable();
        }
        else
        {
            IQueryable<T> queryable2 = dbSet.AsQueryable();
            queryable = queryable2;
        }
        IQueryable<T> query = queryable;
        query = (isNoTracked ? query.AsNoTracking() : query);
        if (includes != null)
        {
            foreach (string item in includes)
            {
                query = query.Include(item);
            }
        }
        return query;
    }

    public async Task<T> GetAsync(Expression<Func<T, bool>> expression, string[] includes = null)
    {
        IQueryable<T> query = dbSet.AsQueryable();
        if (includes != null)
        {
            foreach (string item in includes)
            {
                query = query.Include(item);
            }
        }
        return await query.FirstOrDefaultAsync(expression);
    }

    public async Task SaveChanges()
    {
        await appDbContext.SaveChangesAsync();
    }
}
