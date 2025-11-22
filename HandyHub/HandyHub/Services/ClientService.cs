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
        public Client? GetClientWithUserById(int id)
        {
            return clientRepo.GetClientWithUserById(id);
        }

        public List<Client> GetAllWithUser()
        {
            return clientRepo.GetAllWithUser();
        }
        public void CreateClientWithUser(Client model)
        {
            var user = model.User!;
            user.Role = "Client";
            clientRepo.AddUser(user);
            clientRepo.Save();

            model.UserId = user.Id;
            model.User = null; 
            model.CreatedAt = DateTime.Now;

            clientRepo.Insert(model);
            clientRepo.Save();
        }


        public void UpdateClientWithUser(Client model)
        {
            var client = clientRepo.GetClientWithUserById(model.Id);
            if (client == null) return;

            client.User.Name = model.User.Name;
            client.User.Email = model.User.Email;
            client.User.Phone = model.User.Phone;
            client.User.City = model.User.City;
            client.User.PasswordHash = model.User.PasswordHash;

            clientRepo.Update(client);
            clientRepo.Save();
        }

        public void DeleteClientWithUser(int id)
        {
            var client = clientRepo.GetClientWithUserById(id);
            if (client == null) return;

            clientRepo.DeleteUser(client.User);
            clientRepo.Delete(id);
            clientRepo.Save();
        }

        internal object GetClientWithUserById(int? id)
        {
            throw new NotImplementedException();
        }
    }
}
