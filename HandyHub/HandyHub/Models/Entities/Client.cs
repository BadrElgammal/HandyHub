using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel.DataAnnotations;

namespace HandyHub.Models.Entities
{
    public class Client
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        [MinLength(3)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^(010|011|012|015)[0-9]{8}$")]

        public string Phone { get; set; }
        public string City { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }
        public string Role { get; set; } = "Client";

        [Required]
        public string ImageUrl { get; set; } = "/images/default-avatar.png";
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int TotalSearches { get; set; }
        public int TotalContacts { get; set; }

        public ICollection<Review>? Reviews { get; set; }
        public ICollection<Favorite>? Favorites { get; set; }
    }
}
