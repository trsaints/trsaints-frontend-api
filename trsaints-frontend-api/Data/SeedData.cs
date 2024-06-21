using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using trsaints_frontend_api.Constants;
using trsaints_frontend_api.Data.Context;
using trsaints_frontend_api.Data.Entities;

namespace trsaints_frontend_api.Data;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider, string adminEmail, string adminPassword)
    {
        using var context = new AppDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>());
        
        var adminId = await EnsureUser(serviceProvider, adminEmail, adminPassword);
        await EnsureRole(serviceProvider, adminId, ResourceOperationsConstants.RoleAdministrators);
    }
    
    private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
                                                  string email, string userPassword)
    {
        var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

        var user = await userManager.FindByEmailAsync(email);
        
        if (user is not null) return user.Id;
        
        user = new ApplicationUser { UserName = email, Email = email};
        await userManager.CreateAsync(user, userPassword);

        return user.Id;
    }

    private static async Task EnsureRole(IServiceProvider serviceProvider, string uid, string role)
    {
        var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
        
        var user = await userManager.FindByIdAsync(uid);
        var isInRole = await userManager.IsInRoleAsync(user, role);
        
        if (!isInRole)
            await userManager.AddToRoleAsync(user, role);
    }
}