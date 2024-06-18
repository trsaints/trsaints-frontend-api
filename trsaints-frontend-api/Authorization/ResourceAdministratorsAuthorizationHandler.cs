using Microsoft.AspNetCore.Authorization.Infrastructure;
using trsaints_frontend_api.Entities;

namespace trsaints_frontend_api.Authorization;

public class ResourceAdministratorsAuthorizationHandler: ResourceRoleAuthorizationHandler<Entity>
{
    public ResourceAdministratorsAuthorizationHandler()
        : base(ResourceOperationsConstants.RoleAdministrators, [
            ResourceOperations.Create,
            ResourceOperations.Read,
            ResourceOperations.Update,
            ResourceOperations.Delete
        ])
    {
    }
    
    
}