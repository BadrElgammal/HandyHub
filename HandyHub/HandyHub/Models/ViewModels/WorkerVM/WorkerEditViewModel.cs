using HandyHub.Models.Entities;

namespace HandyHub.Models.ViewModels.WorkerVM
{
    public class WorkerEditViewModel
    {
        public Worker? Worker { get; set; }
        public List<Client>? Clients { get; set; } = new List<Client>();
        public List<Review>? Review { get; set; }
        public List<WorkerPortfolio>? Portfolio { get; set; }
        public Category Categories { get; set; }

        public IFormFile? ProfileImage { get; set; }
    }
}
