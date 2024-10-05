using Feedback_System.Models;
using Feedback_System.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<ParkEntities> Parks { get; set; }
    public DbSet<PostModel> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Park ve Post arasındaki ilişkiyi yapılandırıyoruz
        modelBuilder.Entity<PostModel>()
            .HasOne(p => p.Park)
            .WithMany(p => p.Posts)
            .HasForeignKey(p => p.ParkId);

        // Identity için gerekli ayarları ekliyoruz
        base.OnModelCreating(modelBuilder);
    }
}
