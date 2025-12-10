using System.ComponentModel.DataAnnotations;

namespace HandyHub.Models.Entities;

public class User
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "الاسم مطلوب")]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "كلمة المرور مطلوبة")]
    [DataType(DataType.Password)]
    public string PasswordHash { get; set; } = string.Empty;
    [RegularExpression(@"^(010|011|012|015)[0-9]{8}$")]
    public string Phone { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;// "Client", "Worker", "Admin"
    public string? ImageUrl { get; set; } = "default-avatar-admin.png";
}
