using System.Linq.Expressions;

namespace VZTest.Data.IRepository
{
    /// <summary>
    /// Интерфейс репозитория для сущностей (Часть реализации паттерна 'Репозиторий')
    /// </summary>
    /// <typeparam name="T">Класс сущности</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Метод асинхронного добавления сущности в базу
        /// </summary>
        /// <param name="value">Новая сущность</param>
        Task AddAsync(T value);
        /// <summary>
        /// Метод асинхронного добавления набора сущностей в базу
        /// </summary>
        /// <param name="value">Набор сущностей</param>
        Task AddRangeAsync(IEnumerable<T> values);
        /// <summary>
        /// Метод удаления сущности из базы
        /// </summary>
        /// <param name="value">Сущность</param>
        void Remove(T value);
        /// <summary>
        /// Метод удаления набора сущностей из базы
        /// </summary>
        /// <param name="value">Набор сущностей</param>
        void RemoveRange(IEnumerable<T> value);
        /// <summary>
        /// Метод поиска сущность в базе по заданному фальтру (Лямбда-выражение)
        /// </summary>
        /// <param name="filter">Фильтр поиска</param>
        /// <returns>Сущность или null</returns>
        T? FirstOrDefault(Expression<Func<T, bool>> filter);
        /// <summary>
        /// Метод подсчёта количества сущностей в таблице
        /// </summary>
        /// <returns>Количество сущностей в таблице</returns>
        Task<int> CountAsync();
        /// <summary>
        /// Метод подсчёта количества сущностей соответствующих фильтру в таблице
        /// </summary>
        /// /// <param name="filter">Фильтр поиска</param>
        /// <returns>Количество сущностей в таблице соответствующих фильтру</returns>
        Task<int> CountAsync(Expression<Func<T, bool>> filter);
        /// <summary>
        /// Метод получения набора сущностей соответствующих фильтру
        /// </summary>
        /// <param name="filter">Фильтр поиска</param>
        /// <returns>Набор сущностей соответствующих фильтру</returns>
        IEnumerable<T> GetWhere(Expression<Func<T, bool>> filter);
        /// <summary>
        /// Метод получения всех сущностей таблицы
        /// </summary>
        /// <returns>Все сущности таблицы</returns>
        IEnumerable<T> GetAll();
        /// <summary>
        /// Метод обновления сущности в базе
        /// </summary>
        /// <param name="value">Сущность</param>
        void Update(T value);
        /// <summary>
        /// Метлд обновления Набора сущностей в базе
        /// </summary>
        /// <param name="value">Набор сущностей</param>
        void UpdateRange(IEnumerable<T> value);
    }
}
