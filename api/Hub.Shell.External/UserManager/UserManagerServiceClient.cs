using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Functional;
using IQ.RestClient.Core;
using Microsoft.Extensions.Configuration;
using Hub.Shell.Configuration;
using System.Threading;
using Hub.Shell.External.UserManager.Resources;

namespace Hub.Shell.External.UserManager
{
    public class UserManagerServiceClient : IUserManagerServiceClient
    {

        private readonly IRestClient _restClient;
        private readonly IConfiguration _configuration;

        public UserManagerServiceClient(IRestClient restClient, IConfiguration configuration)
        {
            _restClient = restClient;
            _configuration = configuration;
        }

        public async Task<Result<UserResource, UserManagerError>> GetUser(int userId)
        {
            var get = _restClient.GetAsync($"{_configuration.Get().UserManagerServiceUrl}/v1/users({userId})");

            var response = await _restClient.Send<UserResource, UserManagerError>(get.Request, CancellationToken.None);

            return response.Match(
                Result.Success<UserResource, UserManagerError>,
                loginError =>
                {
                    throw new Exception(loginError.Description);
                }
            );
        }

        public async Task<Result<IEnumerable<AssignedSecurityRoleResource>, UserManagerError>> GetSecurityRolesForUser(int userId)
        {
            var get = _restClient.GetAsync($"{_configuration.Get().UserManagerServiceUrl}/v1/users({userId})/assignedroles");
            var response = await _restClient.Send<IEnumerable<AssignedSecurityRoleResource>, UserManagerError>(get.Request, CancellationToken.None);

            return response.Match(
                Result.Success<IEnumerable<AssignedSecurityRoleResource>, UserManagerError>,
                error =>
                {
                    throw new Exception(error.Description);
                }
            );
        }

        public async Task<Result<IEnumerable<PermissionResource>, UserManagerError>> GetPermissionsForRole(int entityId, int securityRoleId)
        {
            var get = _restClient.GetAsync($"{_configuration.Get().UserManagerServiceUrl}/v1/entities({entityId})/securityRoles({securityRoleId})/permissions");
            var response = await _restClient.Send<IEnumerable<PermissionResource>, UserManagerError>(get.Request, CancellationToken.None);

            return response.Match(
                Result.Success<IEnumerable<PermissionResource>, UserManagerError>,
                error =>
                {
                    throw new Exception(error.Description);
                }
            );
        }
    }
}