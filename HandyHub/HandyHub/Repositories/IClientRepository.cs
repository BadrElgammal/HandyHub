using HandyHub.Models.Entities;
using static HandyHub.Repositories.IRepository;

namespace HandyHub.Repositories
{
    public interface IClientRepository : IRepository<Client>
    {
        bool IsEmailExist(string email, int? id = null);
        Client? GetClientWithUserById(int id);
        List<Client> GetAllWithUser();
    }
}
