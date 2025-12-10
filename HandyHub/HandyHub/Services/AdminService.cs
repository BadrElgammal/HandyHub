using HandyHub.Models.Entities;
using HandyHub.Repositories;

namespace HandyHub.Services
{
    public class AdminService:GenericService<Admin>,IAdminService
    {
        IAdminRepository adminRepo;
        public AdminService(IAdminRepository _AdminRepo) : base(_AdminRepo)
        {
            adminRepo = _AdminRepo;
        }

        public bool IsEmailExist(string email, int? id = null)
        {
            return adminRepo.IsEmailExist(email, id);
        }
        public Admin? GetAdminWithUserById(int id)
        {
            return adminRepo.GetAdminWithUserById(id);
        }

        public List<Admin> GetAllWithUser()
        {
            return adminRepo.GetAllWithUser();
        }
        public void CreateAdminWithUser(Admin model)
        {
            var user = model.User!;
            user.Role = "Admin";
            adminRepo.AddUser(user);
            adminRepo.Save();

            model.UserId = user.Id;
            model.User = null;
            model.CreatedAt = DateTime.Now;

            adminRepo.Insert(model);
            adminRepo.Save();
        }


        public void UpdateAdminWithUser(Admin model)
        {
            var admin = adminRepo.GetAdminWithUserById(model.Id);
            if (admin == null) return;

            admin.User.Name = model.User.Name;
            admin.User.Email = model.User.Email;
            admin.User.Phone = model.User.Phone;
            admin.User.City = model.User.City;
            admin.User.PasswordHash = model.User.PasswordHash;

            adminRepo.Update(admin);
            adminRepo.Save();
        }

        public void DeleteAdminWithUser(int id)
        {
            var admin = adminRepo.GetAdminWithUserById(id);
            if (admin == null) return;

            adminRepo.DeleteUser(admin.User);
            adminRepo.Delete(id);
            adminRepo.Save();
        }
    }
}
