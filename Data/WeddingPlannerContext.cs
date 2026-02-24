using Microsoft.EntityFrameworkCore;
using WeddingPlanner.Api.Models;
using WeddingPlanner.Api.Models.Enums;

namespace WeddingPlanner.Api.Data
{
    public class WeddingPlannerContext : DbContext
    {
        public WeddingPlannerContext(DbContextOptions<WeddingPlannerContext> options)
    : base(options)
        {
        }

        public DbSet<Wedding> Weddings { get; set; }
        public DbSet<WeddingTask> WeddingTasks { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Wedding>()
                .HasMany(w => w.Tasks)
                .WithOne(t => t.Wedding)
                .HasForeignKey(t => t.WeddingId);

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,           
                Name = "Admin",
                Role = UserRole.Admin
            });
        }
    }
}
