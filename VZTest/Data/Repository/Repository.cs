using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VZTest.Data.IRepository;

namespace VZTest.Data.Repository
{
    /// <summary>
    /// Класс репозитория для сущностей (Часть реализации паттерна 'Репозиторий')
    /// </summary>
    /// <typeparam name="T">Класс сущности</typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        /// <summary>
        /// Набор сущностей из базы
        /// </summary>
        protected DbSet<T> Set { get; set; }

        /// <summary>
        /// Основной конструктор класса
        /// </summary>
        /// <param name="db">Контекст базы данных приложения</param>
        public Repository(ApplicationDbContext db)
        {
            Set = db.Set<T>();
        }

        public async Task AddAsync(T value)
            => await Set.AddAsync(value);

        public async Task AddRangeAsync(IEnumerable<T> values)
            => await Set.AddRangeAsync(values);

        public async Task<int> CountAsync()
            => await Set.CountAsync();

        public async Task<int> CountAsync(Expression<Func<T, bool>> filter)
            => await Set.CountAsync(filter);

        public T? FirstOrDefault(Expression<Func<T, bool>> filter)
            => Set.FirstOrDefault(filter);

        public IEnumerable<T> GetAll()
            => Set;

        public IEnumerable<T> GetWhere(Expression<Func<T, bool>> filter)
            => Set.Where(filter);

        public void Remove(T value)
            => Set.Remove(value);

        public void Update(T value)
            => Set.Update(value);

        public void RemoveRange(IEnumerable<T> value)
            => Set.RemoveRange(value);

        public void UpdateRange(IEnumerable<T> value)
            => Set.UpdateRange(value);
    }
}
