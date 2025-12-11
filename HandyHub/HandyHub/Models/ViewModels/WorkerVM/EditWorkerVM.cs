using HandyHub.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace HandyHub.Models.ViewModels.WorkerVM
{
    public class EditWorkerVM
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        [Required(ErrorMessage = "الاسم مطلوب")]
        [StringLength(100)]
        public string Name { get; set; }
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [RegularExpression(@"^(010|011|012|015)[0-9]{8}$")]
        public string Phone { get; set; }
        public string City { get; set; }
        public string? Area { get; set; }
        public string? Bio { get; set; }
        public int? CategoryId { get; set; }
        //public Worker Worker { get; set; }
        public List<Category> Categorys { get; set; } = new List<Category>();

        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
        public string? ExistingProfileImagePath { get; set; }

        public IFormFile? ProfileImage { get; set; }
    }
}
