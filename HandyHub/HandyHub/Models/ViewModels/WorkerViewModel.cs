using HandyHub.Models.Entities;

namespace HandyHub.Models.ViewModels
{
    public class WorkerWithCatigoryViewModel
    {
        public Worker Worker { get; set; } = new Worker();
        public List<Category> Categorys { get; set; } = new List<Category>();

        public IFormFile? ProfileImage { get; set; }
    }

}
