using Microsoft.AspNetCore.Authorization.Infrastructure;
using trsaints_frontend_api.Constants;

namespace trsaints_frontend_api.Authorization;

public static class ResourceOperations
{
    public static readonly OperationAuthorizationRequirement Create = new() {Name=ResourceOperationsConstants.OperationCreate};
    public static readonly OperationAuthorizationRequirement Read = new() {Name=ResourceOperationsConstants.OperationRead};  
    public static readonly OperationAuthorizationRequirement Update = new() {Name=ResourceOperationsConstants.OperationUpdate}; 
    public static readonly OperationAuthorizationRequirement Delete = new() {Name=ResourceOperationsConstants.OperationDelete};
}

