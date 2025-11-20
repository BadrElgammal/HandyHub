using System.ComponentModel.DataAnnotations;

namespace HandyHub.Models.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة (مثال: user@email.com)")]
        public string Email { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [MinLength(6, ErrorMessage = "يجب ألا تقل كلمة المرور عن 6 أحرف")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "نوع الحساب مطلوب")]
        public string Role { get; set; }
    }
}
