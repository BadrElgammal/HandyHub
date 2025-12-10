using HandyHub.Models.Entities;
using static HandyHub.Repositories.IRepository;

namespace HandyHub.Repositories
{
    public interface IAdminRepository:IRepository<Admin>
    {
        bool IsEmailExist(string email, int? id = null);
        Admin? GetAdminWithUserById(int id);
        List<Admin> GetAllWithUser();
    }
}
