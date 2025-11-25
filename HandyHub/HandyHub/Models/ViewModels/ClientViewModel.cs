using HandyHub.Models.Entities;

namespace HandyHub.Models.ViewModels
{
    public class ClientViewModel
    {    



    }
    public class ClientDashboardVM
    {
        public Client Client { get; set; }

        public List<Review> Reviews { get; set; }
        public List<Favorite> Favorites { get; set; }
        public List<Worker> Workers { get; set; }
    }
}
