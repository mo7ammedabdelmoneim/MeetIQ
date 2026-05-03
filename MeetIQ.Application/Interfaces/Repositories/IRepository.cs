using System.Linq.Expressions;

namespace MeetIQ.Application.Interfaces.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetAsync(Expression<Func<T, bool>>? expression = null);
        Task AddAsync(T entity);
        Task AddRangeAsync(List<T> entities);
        void Update(T entity);
        void Delete(T entity);
        Task SaveChangesAsync();
    }
}
