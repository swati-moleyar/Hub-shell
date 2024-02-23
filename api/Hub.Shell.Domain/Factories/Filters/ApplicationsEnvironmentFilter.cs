using System;
using System.Linq;
using System.Collections.Generic;
using Functional;
using Hub.Shell.Configuration;
using Hub.Shell.Error;
using Hub.Shell.External.Hub.Resources;
using Microsoft.Extensions.Configuration;

namespace Hub.Shell.Domain.Factories.Filters
{
    public class ApplicationsEnvironmentFilter : IApplicationsEnvironmentFilter
    {
        private readonly IConfiguration _configuration;

        public ApplicationsEnvironmentFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Result<IEnumerable<ApplicationResource>, ServiceError> Apply(IEnumerable<ApplicationResource> apps)
        {
            var currentEnvironment = Enum.Parse<EnvironmentLevel>(_configuration.Get().Environment, true);
            return Result.Success<IEnumerable<ApplicationResource>, ServiceError>(
                apps.Where(app => HasCorrectEnvironment(app, currentEnvironment))
            );
        }

        private bool HasCorrectEnvironment(ApplicationResource app, EnvironmentLevel environment)
        {
            if (!string.IsNullOrEmpty(app.EnvironmentToggleLevel))
            {
                if (Enum.TryParse<EnvironmentLevel>(app.EnvironmentToggleLevel, true, out var appEnvironment))
                {
                    return environment <= appEnvironment;
                }
                return false;
            }
            return true;
        }
    }
}
