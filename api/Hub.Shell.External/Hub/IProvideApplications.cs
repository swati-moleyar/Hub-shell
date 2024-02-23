using System.Threading.Tasks;
using Functional;
using Hub.Shell.External.Hub.Resources;

namespace Hub.Shell.External.Hub
{
    public interface IProvideApplications
    {
        Task<Result<ApplicationsResource, string>> GetApplications();
    }
}