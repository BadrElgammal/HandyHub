using HandyHub.Data;
using Microsoft.EntityFrameworkCore;
using HandyHub.Models.Entities;

namespace HandyHub.Repositories
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        private readonly HandyHubDbContext _context;

        public ClientRepository(HandyHubDbContext context) : base(context)
        {
            _context = context;
        }

        public bool IsEmailExist(string email, int? id = null)
        {
            return _context.Users.Any(u => u.Email == email && (id == null || u.Id != id) && u.Role =="Client");
        }
        public Client? GetClientWithUserById(int id)
        {
            return _context.Clients.Include(c => c.User).FirstOrDefault(c => c.Id == id);
        }

        public List<Client> GetAllWithUser()
        {
            return _context.Clients.Include(c => c.User).ToList();
        }
    }
}
