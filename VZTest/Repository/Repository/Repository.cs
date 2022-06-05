using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VZTest.Data;
using VZTest.Repository.IRepository;

namespace VZTest.Repository.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext db;
        private DbSet<T> set;

        public Repository(ApplicationDbContext db)
        {
            this.db = db;
            set = db.Set<T>();
        }

        public async Task AddAsync(T value)
            => await set.AddAsync(value);

        public async Task AddRangeAsync(IEnumerable<T> values)
            => await set.AddRangeAsync(values);

        public T? FirstOrDefault(Expression<Func<T, bool>> filter)
            => set.FirstOrDefault(filter);

        public IEnumerable<T> GetAll()
            => set;

        public IEnumerable<T> GetWhere(Expression<Func<T, bool>> filter)
            => set.Where(filter);

        public void Remove(T value)
            => set.Remove(value);
    }
}
