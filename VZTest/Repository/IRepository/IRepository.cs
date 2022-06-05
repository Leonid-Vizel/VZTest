using System.Linq.Expressions;

namespace VZTest.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        void Add(T value);
        void Remove(T value);
        T? FirstOrDefault(Expression<Func<T, bool>> filter);
        IEnumerable<T> GetAll();
    }
}
