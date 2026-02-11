using Microsoft.EntityFrameworkCore;
using WeddingPlanner.Api.Models;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Wedding>()
                .HasMany(w => w.Tasks)
                .WithOne(t => t.Wedding)
                .HasForeignKey(t => t.WeddingId);
        }
    }
}
