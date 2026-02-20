namespace Api.Databases.Repositories.BaseRepository;

public interface IBaseRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<T?> FindAsync(Func<T, bool> predicate);
    Task<int> CountAsync();
    Task<int> CountAsync(Func<T, bool> predicate);

}