using HandyHub.Models.Entities;
using HandyHub.Repositories;
using static HandyHub.Repositories.IRepository;

namespace HandyHub.Services
{
    public class WorkerService : GenericService<Worker>, IWorkerService
    {
        IWorkerRepository workerRepo;
        public WorkerService(IWorkerRepository _repository) : base(_repository)
        {
            workerRepo = _repository;
        }

        public bool IsEmailExist(string email, int? id = null)
        {
            return workerRepo.IsEmailExist(email, id);
        }
        public bool SuspendWorker(Worker worker)
        {
            bool sus= workerRepo.SuspendWorker(worker);
            workerRepo.Save();
            return sus;
        }
    }
}
