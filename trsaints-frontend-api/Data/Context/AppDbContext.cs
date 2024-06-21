using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using trsaints_frontend_api.Data.Entities;

namespace trsaints_frontend_api.Data.Context;

public class AppDbContext: IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    {
    }
    
    public DbSet<TechStack> TechStacks { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Skill> Skills { get; set;  }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        builder.Entity<Skill>().HasKey(s => s.Id);
        builder.Entity<Skill>().Property(s => s.Name).HasMaxLength(100).IsRequired();
        
        builder.Entity<TechStack>().HasKey(s => s.Id);
        builder.Entity<TechStack>().Property(s => s.Name).HasMaxLength(100).IsRequired();
        builder.Entity<TechStack>().HasMany(s => s.Projects).WithOne(p => p.TechStack).HasForeignKey(p => p.StackId);
        
        builder.Entity<Project>().HasKey(p => p.Id);
        builder.Entity<Project>().Property(p => p.Name).HasMaxLength(100).IsRequired();
        builder.Entity<Project>().Property(p => p.Description).HasMaxLength(200).IsRequired();
        builder.Entity<Project>().Property(p => p.RepoUrl).HasMaxLength(200).IsRequired();
        builder.Entity<Project>().Property(p => p.DeployUrl).HasMaxLength(200).IsRequired();
        builder.Entity<Project>().Property(p => p.Banner).HasMaxLength(250).IsRequired();

        foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
    }
}
