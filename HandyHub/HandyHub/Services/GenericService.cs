using HandyHub.Models.Entities;
using HandyHub.Repositories;
using System.Linq.Expressions;
using static HandyHub.Repositories.IRepository;

namespace HandyHub.Services
{
    public class GenericService<T> : IService<T> where T : class
    {
        protected readonly IRepository<T> _repository;

        public GenericService(IRepository<T> repository)
        {
            _repository = repository;
        }

        public void Delete(object id)
        {
            _repository.Delete(id);
            _repository.Save();
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _repository.Find(predicate);
        }

        public List<T> GetAll()
        {
            return _repository.GetAll();
        }

        public T GetById(object id)
        {
            return _repository.GetById(id);
        }

        public void Insert(T entity)
        {
            _repository.Insert(entity);
            _repository.Save();
        }


        public void Update(T entity)
        {
            _repository.Update(entity);
            _repository.Save();
        }
        public void AddUser(User user)
        {
            _repository.AddUser(user);
        }

        public void DeleteUser(User user)
        {
            _repository.DeleteUser(user);
        }
    }
}
