using trsaints_frontend_api.Entities;

namespace trsaints_frontend_api.Authorization;

public class ResourceUsersAuthorizationHandler: ResourceRoleAuthorizationHandler<Entity>
{
    public ResourceUsersAuthorizationHandler()
        : base(ResourceOperationsConstants.RoleUsers, [
            ResourceOperations.Read
        ])
    {
    }
}