using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using trsaints_frontend_api.Data.Entities;

namespace trsaints_frontend_api.Data.Context;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) :
        base(options)
    {
    }

    public DbSet<TechStack> TechStacks { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Skill> Skills { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly);

        builder.Entity<Skill>().HasKey(s => s.Id);
        builder.Entity<Skill>()
               .Property(s => s.Name)
               .HasMaxLength(128)
               .IsRequired();
        builder.Entity<Skill>()
               .Property(s => s.SkillCategory)
               .IsRequired();

        builder.Entity<TechStack>().HasKey(t => t.Id);
        builder.Entity<TechStack>()
               .Property(t => t.Name)
               .HasMaxLength(128)
               .IsRequired();
        builder.Entity<TechStack>()
               .HasMany(t => t.Projects)
               .WithOne(p => p.TechStack)
               .HasForeignKey(p => p.TechStackId);
        builder.Entity<TechStack>()
               .HasMany<Skill>(s => s.Skills)
               .WithMany(s => s.TechStacks)
               .UsingEntity<TechStackSkill>("TechStacksToSkillsJoinTable");

        builder.Entity<Project>().HasKey(p => p.Id);
        builder.Entity<Project>()
               .Property(p => p.Name)
               .HasMaxLength(128)
               .IsRequired();
        builder.Entity<Project>()
               .Property(p => p.Description)
               .HasMaxLength(256)
               .IsRequired();
        builder.Entity<Project>()
               .Property(p => p.RepoUrl)
               .HasMaxLength(256)
               .IsRequired();
        builder.Entity<Project>()
               .Property(p => p.DeployUrl)
               .HasMaxLength(256)
               .IsRequired();
        builder.Entity<Project>()
               .Property(p => p.Banner)
               .HasMaxLength(256)
               .IsRequired();

        foreach (var relationship in builder.Model.GetEntityTypes()
                                            .SelectMany(
                                                e => e
                                                    .GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
    }
}
