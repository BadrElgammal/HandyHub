using HandyHub.Models.Entities;

namespace HandyHub.Services
{
    public interface IClientService : IService<Client> 
    {
        bool IsEmailExist(string email, int? id = null);
        Client? GetClientWithUserById(int id);
        List<Client> GetAllWithUser();
        void CreateClientWithUser(Client model); // لإنشاء العميل مع المستخدم
        void UpdateClientWithUser(Client model);
        void DeleteClientWithUser(int id);
    }
}
