using Microsoft.AspNetCore.Authorization;

namespace trsaints_frontend_api.Authorization;

public class ResourceRoleAuthorizationHandler<TRequirement, TResource>
    : AuthorizationHandler<TRequirement, TResource> where TRequirement: IAuthorizationRequirement
{
    private readonly string _role;
    
    public ResourceRoleAuthorizationHandler(string role)
    {
        _role = role;
    }
    
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TRequirement requirement, TResource resource)
    {
        if (context.User is null)
            return Task.CompletedTask;

        if (context.User.IsInRole(_role)) 
            context.Succeed(requirement);
        
        return Task.CompletedTask;
    }
}