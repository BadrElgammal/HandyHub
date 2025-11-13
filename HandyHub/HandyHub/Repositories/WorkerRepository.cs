using HandyHub.Data;
using HandyHub.Models.Entities;

namespace HandyHub.Repositories
{
    public class WorkerRepository : GenericRepository<Worker>, IWorkerRepository
    {
        public WorkerRepository(HandyHubDbContext context) : base(context)
        {
        }

        public bool IsEmailExist(string email, int? id = null)
        {
            return _dbSet.Any(c => c.Email == email && (id == null || c.Id != id));
        }

        public bool SuspendWorker(Worker worker)
        {
            return worker.IsAvailable = worker.IsAvailable ? false : true;
        }
    }
}
