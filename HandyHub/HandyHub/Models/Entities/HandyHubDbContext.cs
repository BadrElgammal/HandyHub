
using HandyHub.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HandyHub.Data
{
    public class HandyHubDbContext : DbContext
    {
        public HandyHubDbContext ( DbContextOptions<HandyHubDbContext> options )
            : base(options)
        {
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<WorkerPortfolio> WorkerPortfolio { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating ( ModelBuilder modelBuilder )
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Worker>()
                .HasOne(w => w.Category)
                .WithMany(c => c.Workers)
                .HasForeignKey(w => w.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);


            modelBuilder.Entity<Review>()
                .HasOne(r => r.Client)
                .WithMany(c => c.Reviews)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Worker)
                .WithMany(w => w.Reviews)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}