using System.ComponentModel.DataAnnotations;

namespace HandyHub.Models.ViewModels.ClientVM
{
    public class EditClientVM
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

        public string? ExistingProfileImagePath { get; set; }

        public IFormFile? ProfileImage { get; set; }
    }
}
