using HandyHub.Data;
using Microsoft.EntityFrameworkCore;
using HandyHub.Models.Entities;

namespace HandyHub.Repositories
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        public ClientRepository(HandyHubDbContext context) : base(context)
        {
        }

        public bool IsEmailExist(string email, int? id = null)
        {
            return _dbSet.Any(c => c.Email == email && (id == null || c.Id != id));
        }
    }
}
