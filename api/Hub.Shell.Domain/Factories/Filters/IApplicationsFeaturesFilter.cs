using System.Threading.Tasks;
using System.Collections.Generic;
using Functional;
using Hub.Shell.Error;
using Hub.Shell.External.Hub.Resources;

namespace Hub.Shell.Domain.Factories.Filters
{
    public interface IApplicationsFeaturesFilter
    {
        Task<Result<IEnumerable<ApplicationResource>, ServiceError>> Apply(IEnumerable<ApplicationResource> apps, int entityId);
    }
}