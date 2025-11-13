using HandyHub.Models.Entities;

namespace HandyHub.Services
{
    public interface IWorkerService :IService<Worker>
    {
        bool IsEmailExist(string email, int? id = null);
        bool SuspendWorker(Worker worker);

    }
}
