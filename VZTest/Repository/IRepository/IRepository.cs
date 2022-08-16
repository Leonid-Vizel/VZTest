using System.Linq.Expressions;

namespace VZTest.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T value);
        Task AddRangeAsync(IEnumerable<T> values);
        void Remove(T value);
        T? FirstOrDefault(Expression<Func<T, bool>> filter);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> filter);
        IEnumerable<T> GetWhere(Expression<Func<T, bool>> filter);
        IEnumerable<T> GetAll();
        void Update(T value);
    }
}
