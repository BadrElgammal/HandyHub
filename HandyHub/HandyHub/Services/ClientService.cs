using HandyHub.Data;
using HandyHub.Models.Entities;
using HandyHub.Repositories;

namespace HandyHub.Services
{
    public class ClientService : GenericService<Client>, IClientService
    {
        IClientRepository clientRepo;
        public ClientService(IClientRepository _clientRepo) : base(_clientRepo)
        {
            clientRepo = _clientRepo;
        }

        public bool IsEmailExist(string email, int? id = null)
        {
            return clientRepo.IsEmailExist(email, id);
        }
    }
}
