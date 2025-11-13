using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HandyHub.Models.Entities
{
    public class WorkerPortfolio
    {
        [Key]
        public int Id { get; set; }
        public string? Bio { get; set; }

        [StringLength(500)]
        public string? Area { get; set; }
        [Required]
        public string ImageUrl { get; set; }

        [ForeignKey("Worker")]
        public int WorkerId { get; set; }
        public Worker Worker { get; set; }
    }
}
