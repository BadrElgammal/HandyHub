using HandyHub.Models.Entities;

namespace HandyHub.Services
{
    public interface IClientService : IService<Client> 
    {
        bool IsEmailExist(string email, int? id = null);
    }
}
