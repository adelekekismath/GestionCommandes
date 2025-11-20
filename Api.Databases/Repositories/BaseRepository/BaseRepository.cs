namespace Api.Databases.Repositories.BaseRepository;
using Microsoft.EntityFrameworkCore;

using Api.Databases.Contexts;

public class BaseRepository<T>(AppDbContext db) : IBaseRepository<T> where T : class
{
    protected readonly AppDbContext _db = db;
    protected  DbSet<T> DbSet => _db.Set<T>();


    public async Task<T> GetByIdAsync(int id) => await DbSet.FindAsync(id);
    public async Task<IEnumerable<T>> GetAllAsync() => await DbSet.ToListAsync();
    public async Task<T> AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
        return entity;
    }
    public async Task<T> UpdateAsync(T entity)
    {
        DbSet.Update(entity);
        return entity;
    }
    public async Task DeleteAsync(T entity)
    {
        DbSet.Remove(entity);
    }
}