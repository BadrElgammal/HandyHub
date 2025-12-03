namespace HandyHub.Models.ViewModels.WorkerVM
{
    public class EditWorkerVM
    {
        public int Id { get; set; }

        public string? Area { get; set; }
        public string? Bio { get; set; }
        public bool IsAvailable { get; set; }
        public int? CategoryId { get; set; }

        public string? ExistingProfileImagePath { get; set; }

        public IFormFile? ProfileImage { get; set; }
    }
}
