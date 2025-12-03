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
        public string ImageUrl { get; set; } = "https://www.google.com/imgres?q=%D8%B4%D8%BA%D9%84%20%D8%B3%D8%A8%D8%A7%D9%83%D9%87%20%D8%AC%D8%A7%D9%87%D8%B2%20%D8%A7%D9%88%20%D9%86%D8%AC%D8%A7%D8%B1%D9%87&imgurl=https%3A%2F%2Fwww.company-saudi.com%2Fwp-content%2Fuploads%2F2023%2F05%2Fcarpenter-in-jeddah.png&imgrefurl=https%3A%2F%2Fwww.company-saudi.com%2Fcarpenter-in-jeddah%2F&docid=4bxP7tMiIeZdlM&tbnid=fi2DSpWaIitL1M&vet=12ahUKEwiv-cWf96GRAxW5KvsDHSJNLucQM3oECFgQAA..i&w=709&h=425&hcb=2&ved=2ahUKEwiv-cWf96GRAxW5KvsDHSJNLucQM3oECFgQAA";

        [ForeignKey("Worker")]

        public int WorkerId { get; set; }
        [BindNever]
        public Worker ?Worker { get; set; }
    }
}
