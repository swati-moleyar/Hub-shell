using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Functional;
using Hub.Shell.Domain.Mapping;
using Hub.Shell.Domain.Factories.Filters;
using Hub.Shell.Contracts;
using Hub.Shell.Error;
using Hub.Shell.External.EntityStore;
using Hub.Shell.External.EntityStore.Resources;
using Hub.Shell.External.UserManager;
using Hub.Shell.External.UserManager.Resources;
using Hub.Shell.External.Hub;
using Hub.Shell.External.Hub.Resources;

namespace Hub.Shell.Domain.Factories
{
    public class ApplicationsFactory : IApplicationsFactory
    {
        private readonly IProvideApplications _applications;

        private readonly IApplicationsEnvironmentFilter _environmentFilter;
        private readonly IApplicationsPermissionsFilter _permissionsFilter;
        private readonly IApplicationsFeaturesFilter _featuresFilter;

        public ApplicationsFactory(
            IProvideApplications applications,
            IApplicationsEnvironmentFilter environmentFilter,
            IApplicationsPermissionsFilter permissionsFilter,
            IApplicationsFeaturesFilter featuresFilter)
        {
            _applications = applications;
            _environmentFilter = environmentFilter;
            _permissionsFilter = permissionsFilter;
            _featuresFilter = featuresFilter;
        }

        public Task<Result<IEnumerable<ApplicationGroupContract>, ServiceError>> GetApplications(int userId, int entityId, string entityRole, string origin)
        {
            return 
                from appsResource in GetApplicationsResource()
                from userGroups in GetGroupsFromResource(appsResource, entityRole)
                from userApps in GetApplicationsFromResource(appsResource, userId, entityId)
                select userGroups.Select(x => x.ToContract(userApps, origin)).Where(x => x != null);
        }

        private Task<Result<ApplicationsResource, ServiceError>> GetApplicationsResource()
        {
            return _applications.GetApplications().Match(
                applicationsResource => Result.Success<ApplicationsResource, ServiceError>(applicationsResource),
                error => Result.Failure<ApplicationsResource, ServiceError>(new ServiceError(ErrorType.BadRequestData, error))
            );
        }

        private Result<IEnumerable<ApplicationGroupResource>, ServiceError> GetGroupsFromResource(ApplicationsResource appResource, string entityRole)
        {
            if (appResource.ApplicationGroupVariations.TryGetValue(entityRole, out var groupVariationForEntityRole))
            {
                return Result.Success<IEnumerable<ApplicationGroupResource>, ServiceError>(groupVariationForEntityRole);
            }
            return Result.Success<IEnumerable<ApplicationGroupResource>, ServiceError>(appResource.ApplicationGroups);
        }

        private Task<Result<IEnumerable<ApplicationResource>, ServiceError>> GetApplicationsFromResource(ApplicationsResource appResource, int userId, int entityId)
        {
            var apps = appResource.Applications.Values;
            return _environmentFilter.Apply(apps)
                .BindAsync(apps => _permissionsFilter.Apply(apps, userId, entityId))
                .BindAsync(apps => _featuresFilter.Apply(apps, entityId));
        }
    }
}