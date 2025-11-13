using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HandyHub.Models.Entities;


namespace HandyHub.Models.Entities
{
    public class Worker
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
        public string Role { get; set; } = "Worker";
        public string Area { get; set; }
        public string Bio { get; set; }
        public string ImageUrl { get; set; } = "/images/default-avatar.png";
        public bool IsAvailable { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;


        [ForeignKey("Category")]
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public ICollection<Review>? Reviews { get; set; }
        public ICollection<WorkerPortfolio>? Portfolio { get; set; }
    }
}