using HandyHub.Models.Entities;

namespace HandyHub.Models.ViewModels.ClientVM
{
    public class ClientDashboardVM
    {

        public Client Client { get; set; }

        public int TotalBookings { get; set; } = 0; // مش موجودة فعليًا، حطينا dummy
        public int TotalFavorites { get; set; }
        public int TotalReviews { get; set; }
        public decimal TotalSpending { get; set; } = 0; // مش موجودة فعليًا

        public List<Worker>? FavoriteWorkers { get; set; }
        public List<Review>? Reviews { get; set; }

    }
}
