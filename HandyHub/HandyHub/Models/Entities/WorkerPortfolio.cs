using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        public string ImageUrl { get; set; } = "https://images.unsplash.com/photo-1504328345606-18bbc8c9d7d1?auto=format&fit=crop&q=80&w=1000";
        public int WorkerId { get; set; }
        [BindNever]
        public Worker ?Worker { get; set; }
    }
}
