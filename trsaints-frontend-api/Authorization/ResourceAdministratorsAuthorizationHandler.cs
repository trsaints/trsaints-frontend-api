using Microsoft.AspNetCore.Authorization.Infrastructure;
using trsaints_frontend_api.Entities;

namespace trsaints_frontend_api.Authorization;

public class ResourceAdministratorsAuthorizationHandler: ResourceRoleAuthorizationHandler<OperationAuthorizationRequirement, Entity>
{
    public ResourceAdministratorsAuthorizationHandler() : base(ResourceOperationConstants.RoleAdministrators)
    {
    }
}