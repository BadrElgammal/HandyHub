using System.Linq.Expressions;

namespace HandyHub.Repositories
{
    public interface IRepository
    {
        public interface IRepository<T> where T : class
        {
            List<T> GetAll();
            T GetById(object id);
            void Insert(T entity);
            void Update(T entity);
            void Delete(object id);
            void Save();
            IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        }
    }
}
