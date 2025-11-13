using HandyHub.Models.Entities;

namespace HandyHub.Models.ViewModels
{
        public class AdminDashboardViewModel
        {
            public int TotalClients { get; set; }
            public int TotalWorkers { get; set; }
            public int TotalCategories { get; set; }
            public int TotalReviews { get; set; }
            public double AverageRating { get; set; }
            public List<Worker> RecentWorkers { get; set; }
            public List<Client> RecentClients { get; set; }
        }

        public class AdminReportViewModel
        {
            public int TotalClients { get; set; }
            public int TotalWorkers { get; set; }
            public int TotalReviews { get; set; }
            public double AverageRating { get; set; }
            public List<Worker> TopWorkers { get; set; }
            public List<Category> Categories { get; set; }
        }

}
