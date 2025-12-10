using HandyHub.Models.Entities;

namespace HandyHub.Services
{
    public interface IAdminService :IService<Admin>
    {
        bool IsEmailExist(string email, int? id = null);
        Admin? GetAdminWithUserById(int id);
        List<Admin> GetAllWithUser();
        void CreateAdminWithUser(Admin model); 
        void UpdateAdminWithUser(Admin model);
        void DeleteAdminWithUser(int id);
    }
}
