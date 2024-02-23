using System.Collections.Generic;
using System.Threading.Tasks;
using Functional;
using Hub.Shell.External.UserManager.Resources;

namespace Hub.Shell.External.UserManager
{
    public interface IUserManagerServiceClient
    {
        Task<Result<UserResource, UserManagerError>> GetUser(int userId);
        Task<Result<IEnumerable<AssignedSecurityRoleResource>, UserManagerError>> GetSecurityRolesForUser(int userId);
        Task<Result<IEnumerable<PermissionResource>, UserManagerError>> GetPermissionsForRole(int entityId, int securityRoleId);
    }
}