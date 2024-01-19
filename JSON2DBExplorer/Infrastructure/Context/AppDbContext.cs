using JSON2DBExplorer.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace JSON2DBExplorer.Infrastructure.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options
    ) : base(options)
    {
    }

    public DbSet<Configuration> Configurations { get; set; }
    public DbSet<ConfigurationRelationship> ConfigurationRelationships { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ConfigurationRelationship>()
            .HasKey(x => new { x.ParentID, x.ChildID });

        modelBuilder.Entity<ConfigurationRelationship>()
            .HasOne(x => x.Parent)
            .WithMany(x => x.Parents)
            .HasForeignKey(x => x.ParentID)
            .OnDelete(DeleteBehavior.NoAction);
    
        modelBuilder.Entity<ConfigurationRelationship>()
            .HasOne(x => x.Child)
            .WithMany(x => x.Childrens)
            .HasForeignKey(x => x.ChildID)
            .OnDelete(DeleteBehavior.NoAction);
    }
}