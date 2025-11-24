using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HandyHub.Models.Entities;


namespace HandyHub.Models.Entities
{
    public class Worker
    {
        [Key]
        public int Id { get; set; } 
        public string? Area { get; set; }
        public string? Bio { get; set; }
        public bool IsAvailable { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;


        [ForeignKey("Category")]
        public int? CategoryId { get; set; } = 2;
        public Category? Category { get; set; }

        public ICollection<Review>? Reviews { get; set; }
        public ICollection<WorkerPortfolio>? Portfolio { get; set; }

        [ForeignKey("User")]
        public int? UserId { get; set; }

        public User? User { get; set; }
    }
}