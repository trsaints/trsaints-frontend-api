using trsaints_frontend_api.Constants;
using trsaints_frontend_api.Data.Entities;

namespace trsaints_frontend_api.Authorization.Middleware;

public class ResourceUsersAuthorizationHandler: ResourceRoleAuthorizationHandler<Entity>
{
    public ResourceUsersAuthorizationHandler()
        : base(ResourceOperationsConstants.RoleUsers, [
            ResourceOperations.Read
        ])
    {
    }
}