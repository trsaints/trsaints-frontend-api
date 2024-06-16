using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace trsaints_frontend_api.Authorization;

public class ResourceRoleAuthorizationHandler<TResource>
    : AuthorizationHandler<OperationAuthorizationRequirement, TResource> 
{
    private readonly string _role;
    private readonly OperationAuthorizationRequirement[] _operations;
    
    public ResourceRoleAuthorizationHandler(string role, OperationAuthorizationRequirement[] operations)
    {
        _role = role;
        _operations = operations;
    }
    
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, TResource resource)
    {
        if (context.User is null)
            return Task.CompletedTask;

        var operationIsAllowed = _operations.Contains(requirement);
        var hasRole = context.User.IsInRole(_role);
        
        if (hasRole && operationIsAllowed)
            context.Succeed(requirement);
        
        return Task.CompletedTask;
    }
}