using HandyHub.Models.Entities;

namespace HandyHub.Services
{
    public interface IWorkerService :IService<Worker>
    {
        bool IsEmailExist(string email, int? id = null);
        bool SuspendWorker(Worker worker);
        Worker? GetWorkerWithUserById(int id);
        List<Worker> GetAllWithUser();
        void CreateWorkerWithUser(Worker model);
        void UpdateWorkerWithUser(Worker model);
        void DeleteWorkerWithUser(int id);
        List<Worker> GetAllWorkersWithPortfolioWithUserWithReviews();
        Worker GetAllWorkersWithPortfolioWithUserWithReviews(int id);
    }
}
