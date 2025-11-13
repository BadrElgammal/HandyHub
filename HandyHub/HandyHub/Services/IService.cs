using System.Linq.Expressions;

namespace HandyHub.Services
{
    public interface IService<T> where T : class
    {
        List<T> GetAll();
        T GetById(object id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(object id);
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
    }
}
