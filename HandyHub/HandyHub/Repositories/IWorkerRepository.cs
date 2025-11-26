using HandyHub.Models.Entities;
using static HandyHub.Repositories.IRepository;

namespace HandyHub.Repositories
{
    public interface IWorkerRepository : IRepository<Worker>
    {
        bool IsEmailExist(string email, int? id = null);
        bool SuspendWorker(Worker worker);
        Worker? GetWorkerWithUserById(int id);
        List<Worker> GetAllWithUser();
        List<Worker> GetAllWorkersWithPortfolioWithUserWithReviews();
        Worker? GetByid();

       Worker GetAllWorkersWithPortfolioWithUserWithReviews(int id);
    }
}
