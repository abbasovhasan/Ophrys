using Feedback_System.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<ParkEntities> Parks { get; set; }
    public DbSet<PostModel> Posts { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PostModel>()
            .HasOne(p => p.Park)
            .WithMany(p => p.Posts)
            .HasForeignKey(p => p.ParkId);

        base.OnModelCreating(modelBuilder);
    }

}
