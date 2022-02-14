using Microsoft.EntityFrameworkCore;

namespace fy21_simplemtapp.Model
{
    public class Subscription
    {
        public bool IsActive { get; set; } = false;
        public string Id { get; set; } = "1CD21362-9CED-42EB-8958-F965F67C328F";
        public string PartitionKey { get; set; } = "CEE";
    }

    public class DatabaseContext : DbContext
    {
        public DbSet<Subscription> Subscriptions{ get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> context) : base(context) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultContainer("aad");
            modelBuilder.Entity<Subscription>().ToContainer("aad").HasPartitionKey(o=>o.PartitionKey).HasKey(o=>o.Id);
        }
    }
}
