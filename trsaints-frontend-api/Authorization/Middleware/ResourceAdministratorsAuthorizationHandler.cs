using trsaints_frontend_api.Constants;
using trsaints_frontend_api.Data.Entities;

namespace trsaints_frontend_api.Authorization.Middleware;

public class
    ResourceAdministratorsAuthorizationHandler :
    ResourceRoleAuthorizationHandler<Entity>
{
    public ResourceAdministratorsAuthorizationHandler()
        : base(ResourceOperationsConstants.RoleAdministrators,
               [
                   ResourceOperations.Create,
                   ResourceOperations.Read,
                   ResourceOperations.Update,
                   ResourceOperations.Delete
               ])
    {
    }
}
