using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Functional;
using Hub.Shell.Error;
using Hub.Shell.External.UserManager;
using Hub.Shell.External.UserManager.Resources;
using Hub.Shell.External.Hub.Resources;

namespace Hub.Shell.Domain.Factories.Filters
{
    public class ApplicationsPermissionsFilter : IApplicationsPermissionsFilter
    {
        private readonly IUserManagerServiceClient _userManager;

        public ApplicationsPermissionsFilter(IUserManagerServiceClient userManager)
        {
            _userManager = userManager;
        }

        public Task<Result<IEnumerable<ApplicationResource>, ServiceError>> Apply(IEnumerable<ApplicationResource> apps, int userId, int entityId)
        {
            return GetUserPermissions(userId, entityId)
                .Map(userPermissions => userPermissions.Select(x => x.Code).ToHashSet())
                .Map(userPermissionCodes => apps.Where(app => HasPermissions(app, userPermissionCodes)));
        }

        private Task<Result<IEnumerable<PermissionResource>, ServiceError>> GetUserPermissions(int userId, int entityId)
        {
            return _userManager.GetSecurityRolesForUser(userId).Match(
                securityRoles => Result.Success<IEnumerable<AssignedSecurityRoleResource>, ServiceError>(securityRoles),
                userManagerError => Result.Failure<IEnumerable<AssignedSecurityRoleResource>, ServiceError>(new ServiceError(ErrorType.BadRequestData, userManagerError.Message))
            )
            .BindAsync(securityRoles => {
                var results = securityRoles.Select(role => _userManager.GetPermissionsForRole(entityId, role.SecurityRoleId));
                return results.Aggregate(
                    Task.FromResult(Result.Success<IEnumerable<PermissionResource>, ServiceError>(new List<PermissionResource>())),
                    (current, next) => 
                        current.BindAsync(allPermissions => next.Match(
                            permissionsForRole => Result.Success<IEnumerable<PermissionResource>, ServiceError>(allPermissions.Concat(permissionsForRole)),
                            userManagerError => Result.Failure<IEnumerable<PermissionResource>, ServiceError>(new ServiceError(ErrorType.BadRequestData, userManagerError.Message)))
                    )
                );
            })
            .Map(permissions => permissions.GroupBy(x => x.Id).Select(x => x.First()));
        }

        private bool HasPermissions(ApplicationResource app, HashSet<string> permissions)
        {
            if (app.Permissions != null && app.Permissions.Any())
            {
                var commonPermissions = app.Permissions.Intersect(permissions, StringComparer.OrdinalIgnoreCase);
                if (app.AssertionType?.ToLower() == "all")
                {
                    return commonPermissions.Count() == app.Permissions.Count();
                }
                return commonPermissions.Any();
            }
            return true;
        }
    }
}