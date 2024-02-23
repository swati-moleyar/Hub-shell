using System.Threading.Tasks;
using System.Collections.Generic;
using Functional;
using Hub.Shell.Contracts;
using Hub.Shell.Error;

namespace Hub.Shell.Domain.Factories
{
    public interface IApplicationsFactory
    {
        Task<Result<IEnumerable<ApplicationGroupContract>, ServiceError>> GetApplications(int userId, int entityId, string entityRole, string origin);
    }
}