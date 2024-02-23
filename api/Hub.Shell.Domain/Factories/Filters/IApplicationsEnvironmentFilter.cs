using System.Collections.Generic;
using Functional;
using Hub.Shell.Error;
using Hub.Shell.External.Hub.Resources;

namespace Hub.Shell.Domain.Factories.Filters
{
    public interface IApplicationsEnvironmentFilter
    {
        Result<IEnumerable<ApplicationResource>, ServiceError> Apply(IEnumerable<ApplicationResource> apps);
    }
}