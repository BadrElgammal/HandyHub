using HandyHub.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace HandyHub.Models.ViewModels.WorkerVM
{
    public class EditWorkerVM
    {
        //public string Name { get; set; } = string.Empty;
        //public string Email { get; set; } = string.Empty;
        //public string Phone { get; set; } = string.Empty;
        //public string City { get; set; } = string.Empty;
        //public string? Area { get; set; }
        //public string? Bio { get; set; }
        public Worker Worker { get; set; }
        public List<Category> Categorys { get; set; } = new List<Category>();

        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
        public string? ExistingProfileImagePath { get; set; }

        public IFormFile? ProfileImage { get; set; }
    }
}
