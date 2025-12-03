using HandyHub.Models.Entities;

namespace HandyHub.Models.ViewModels
{
    public class ClientViewModel
    {

        public int Id { get; set; }
        public string Name { get; set; }

        public IFormFile ProfileImage { get; set; }
        public string ProfileImagePath { get; set; }

    }
    public class ClientDashboardVM
    {
        public Client Client { get; set; }

        public List<Review> Reviews { get; set; }
        public List<Favorite> Favorites { get; set; }
        public List<Worker> Workers { get; set; }
    }
}
