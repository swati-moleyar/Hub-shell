using System;
using System.Threading.Tasks;
using Functional;
using Hub.Shell.Domain.Factories;
using Hub.Shell.Contracts;
using Hub.Shell.Error;
using Hub.Shell.External.Auth;
using Hub.Shell.External.UserManager;
using IQ.AspNetCore.Auth.IoC.Microsoft.AccessToken;
using IQ.Auth.OAuth2.ProtectedResource.User;
using Hub.Shell.External.EntityStore;
using Hub.Shell.External.UserManager.Resources;
using Hub.Shell.External.EntityStore.Resources;
using Hub.Shell.External.Auth.Resources;
using System.Collections.Generic;

namespace Hub.Shell.Domain.Services
{
    public class SessionService : ISessionService
    {
        private static string IqmetrixLogo = $"data:image/png;base64,{Convert.ToBase64String(Resources.LogoPngs.Iqmetrix)}";
        private static string CovaLogo = $"data:image/png;base64,{Convert.ToBase64String(Resources.LogoPngs.Cova)}";

        private readonly IAuthServiceClient _authClient;
        private readonly IUserManagerServiceClient _userManagerClient;
        private readonly IApplicationsFactory _applications;
        private readonly IGetCurrentUserAsync _accessTokenDataExtractor;
        private readonly IProvideAuthToken _authTokenProvider;
        private readonly IEntityStoreServiceClient _entityStore;

        public SessionService(
            IProvideAuthToken authTokenProvider,
            IGetCurrentUserAsync accessTokenDataExtractor, 
            IAuthServiceClient authClient, 
            IUserManagerServiceClient userManagerClient,
            IApplicationsFactory applications,
            IEntityStoreServiceClient entityStore)
        {
            _authTokenProvider = authTokenProvider;
            _accessTokenDataExtractor = accessTokenDataExtractor;
            _authClient = authClient;
            _userManagerClient = userManagerClient;
            _applications = applications;
            _entityStore = entityStore;
        }

        public async Task<Result<SessionContract, ServiceError>> GetSession(string origin)
        {
            // TODO: We can avoid all these BindOnFailures if the base call returns ServiceError
            var user = await _accessTokenDataExtractor.GetCurrentUserAsync(_authTokenProvider.AuthToken);

            var authConfigTask = _authClient.GetAuthenticationConfiguration(user.ParentEntityId).BindOnFailure(
                error => Result.Failure<AuthenticationConfigurationResource, ServiceError>(new ServiceError(ErrorType.BadRequestData, error.Message))
            );

            var userDetailsTask = _userManagerClient.GetUser(user.UserId).BindOnFailure(
                error => Result.Failure<UserResource, ServiceError>(new ServiceError(ErrorType.BadRequestData, error.Message))
            );
            var parentEntityTask = _entityStore.GetEntity(user.ParentEntityId).BindOnFailure(
                error => Result.Failure<EntityResource, ServiceError>(new ServiceError(ErrorType.BadRequestData, error.Message))
            );

            return await
                (from userDetails in userDetailsTask
                from parentEntity in parentEntityTask
                from authConfig in authConfigTask
                from apps in _applications.GetApplications(user.UserId, parentEntity.Id, parentEntity.Role, origin).BindOnFailure(
                    error => Result.Failure<IEnumerable<ApplicationGroupContract>, ServiceError>(new ServiceError(ErrorType.BadRequestData, error.Message))
                )
                 select new SessionContract
                {
                    UserId = userDetails.Id,
                    CompanyId = parentEntity.Id,
                    CompanyName = parentEntity.Name,
                    ParentEntityRole = parentEntity.Role,
                    FirstName = userDetails.FirstName,
                    LastName = userDetails.LastName,
                    BrandingLogo = GetLogo(origin),
                    CanChangePassword = !authConfig.IsEnabled,
                    ApplicationGroups = apps
                });
        }

        private static string GetLogo(string origin)
        {
            if (Uri.TryCreate(origin, UriKind.Absolute, out Uri uri) && uri.Host.Contains("covasoft"))
            {
                return CovaLogo;
            }

            return IqmetrixLogo;
        }
    }
}