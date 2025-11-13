using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HandyHub.Models.Entities
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("Client")]
        public int ClientId { get; set; }
        public Client Client { get; set; }

        [ForeignKey("Worker")]
        public int WorkerId { get; set; }
        public Worker Worker { get; set; }
    }
}
