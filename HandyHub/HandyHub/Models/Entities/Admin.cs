using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HandyHub.Models.Entities
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsSuperAdmin { get; set; } = false;
        public bool IsActive { get; set; } = true;

        [ForeignKey("User")]
        public int? UserId { get; set; }

        public User? User { get; set; }
    }
}