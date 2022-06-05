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

        public void Add(T value)
            => set.Add(value);

        public T? FirstOrDefault(Expression<Func<T, bool>> filter)
            => set.FirstOrDefault(filter);

        public IEnumerable<T> GetAll()
            => set;

        public void Remove(T value)
            => set.Remove(value);
    }
}
