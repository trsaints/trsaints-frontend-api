using Microsoft.EntityFrameworkCore;
using trsaints_frontend_api.Entities;

namespace trsaints_frontend_api.Context;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    {
    }
    
    public DbSet<Stack> Stacks { get; set; }
    public DbSet<Project> Projects { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        builder.Entity<Skill>().HasKey(s => s.Id);
        builder.Entity<Skill>().Property(s => s.Name).HasMaxLength(100).IsRequired();
        
        builder.Entity<Stack>().HasKey(s => s.Id);
        builder.Entity<Stack>().Property(s => s.Name).HasMaxLength(100).IsRequired();
        builder.Entity<Stack>().HasMany(s => s.Projects).WithOne(p => p.Stack).HasForeignKey(p => p.StackId);
        
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
