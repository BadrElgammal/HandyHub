using System.ComponentModel.DataAnnotations;

namespace HandyHub.Models.ViewModels.ClientVM
{
    public class EditClientVM
    {
        [Key]
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


        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }

        public string? ExistingProfileImagePath { get; set; }

        public IFormFile? ProfileImage { get; set; }
    }
}
