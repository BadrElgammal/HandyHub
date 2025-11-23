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
        public Worker? GetWorkerWithUserById(int id)
        {
            return workerRepo.GetWorkerWithUserById(id);
        }

        public List<Worker> GetAllWithUser()
        {
            return workerRepo.GetAllWithUser();
        }
        public void CreateWorkerWithUser(Worker model)
        {
            var user = model.User!;
            user.Role = "Worker";
            workerRepo.AddUser(user);
            workerRepo.Save();

            model.UserId = user.Id;
            model.User = null;     
            model.CreatedAt = DateTime.Now;
            model.IsAvailable = true;

            workerRepo.Insert(model);
            workerRepo.Save();
        }
        public void UpdateWorkerWithUser(Worker model)
        {
            var worker = workerRepo.GetWorkerWithUserById(model.Id);
            if (worker == null) return;

            worker.User.Name = model.User.Name;
            worker.User.Email = model.User.Email;
            worker.User.Phone = model.User.Phone;
            worker.User.City = model.User.City;
            worker.User.PasswordHash = model.User.PasswordHash;
            worker.Area = model.Area;
            worker.Bio = model.Bio;
            worker.CategoryId = model.CategoryId;

            workerRepo.Update(worker);
            workerRepo.Save();
        }
        public void DeleteWorkerWithUser(int id)
        {
            var worker = workerRepo.GetWorkerWithUserById(id);
            if (worker == null) return;

            workerRepo.DeleteUser(worker.User);
            workerRepo.Delete(id);
            workerRepo.Save();
        }
        public List<Worker> GetAllWorkersWithPortfolioWithUserWithReviews()
        {
            return workerRepo.GetAllWorkersWithPortfolioWithUserWithReviews();
        }
    }
}
