using System;
using System.ComponentModel.DataAnnotations;

namespace HandyHub.Models.Entities
{
    public class Admin
    {
        [Key]
        public int Id { get; set; } 

        [Required(ErrorMessage = "الاسم مطلوب")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }

        public string Role { get; set; } = "Admin";

        [RegularExpression(@"^(010|011|012|015)[0-9]{8}$")]
        public string PhoneNumber { get; set; }

        public string ImageUrl { get; set; } = "/images/default-avatar-admin.png";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsSuperAdmin { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }
}