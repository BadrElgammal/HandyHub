using HandyHub.Data;
using HandyHub.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HandyHub.Repositories
{
    public class WorkerRepository : GenericRepository<Worker>, IWorkerRepository
    {
        private readonly HandyHubDbContext _context;

        public WorkerRepository(HandyHubDbContext context) : base(context)
        {
            _context = context;
        }

        public bool IsEmailExist(string email, int? id = null)
        {
            return _context.Users.Any(u => u.Email == email && (id == null || u.Id != id) && u.Role == "Worker");
        }

        public bool SuspendWorker(Worker worker)
        {
            return worker.IsAvailable = !worker.IsAvailable ;
        }
        public Worker? GetWorkerWithUserById(int id)
        {
            return _context.Workers.Include(c => c.User).Include(c => c.Category).FirstOrDefault(c => c.Id == id);
        }

        public List<Worker> GetAllWithUser()
        {
            return _context.Workers.Include(w => w.User).ToList();
        }

        public List<Worker> GetAllWorkersWithPortfolioWithUserWithReviews()
        {
            return _context.Workers.Include(w => w.Portfolio).Include(u=>u.User).Include(c=>c.Category).Include(r => r.Reviews).ToList();
        }
        public Worker GetAllWorkersWithPortfolioWithUserWithReviews(int id)
        {
            return _context.Workers.Include(w => w.Portfolio).Include(u => u.User).Include(c => c.Category).Include(r => r.Reviews).FirstOrDefault(w=>w.Id==id);

        }

        public Worker? GetByid()
        {
            throw new NotImplementedException();
        }
    }
}
