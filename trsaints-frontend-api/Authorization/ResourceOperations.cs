using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace trsaints_frontend_api.Authorization;

public static class ResourceOperations
{
    public static readonly OperationAuthorizationRequirement Create = new() {Name=ResourceOperationsConstants.OperationCreate};
    public static readonly OperationAuthorizationRequirement Read = new() {Name=ResourceOperationsConstants.OperationRead};  
    public static readonly OperationAuthorizationRequirement Update = new() {Name=ResourceOperationsConstants.OperationUpdate}; 
    public static readonly OperationAuthorizationRequirement Delete = new() {Name=ResourceOperationsConstants.OperationDelete};
}

public static class ResourceOperationsConstants
{
    public const string OperationCreate = "Create";
    public const string OperationRead = "Read";
    public const string OperationUpdate = "Update";
    public const string OperationDelete = "Delete";
    
    public const string RoleAdministrators = "ResourceAdministrator";
    public const string RoleUsers = "ResourceUser";
}