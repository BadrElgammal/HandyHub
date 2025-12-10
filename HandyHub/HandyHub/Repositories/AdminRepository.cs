using HandyHub.Data;
using HandyHub.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HandyHub.Repositories
{
    public class AdminRepository : GenericRepository<Admin>, IAdminRepository
    {
        private readonly HandyHubDbContext _context;

        public AdminRepository(HandyHubDbContext context) : base(context)
        {
            _context = context;
        }

        public bool IsEmailExist(string email, int? id = null)
        {
            return _context.Users.Any(u => u.Email == email && (id == null || u.Id != id) && u.Role == "Admin");
        }

        public List<Client> GetAllWithUser()
        {
            return _context.Clients.Include(c => c.User).ToList();
        }

        public Admin? GetAdminWithUserById(int id)
        {
            return _context.Admins.Include(a => a.User).FirstOrDefault(a => a.Id == id);
        }

        List<Admin> IAdminRepository.GetAllWithUser()
        {
            return _context.Admins.Include(a => a.User).ToList();
        }
    }
}
