using System.Threading.Tasks;
using System.Collections.Generic;
using Functional;
using Hub.Shell.External.FeatureToggles.Resources;

namespace Hub.Shell.External.FeatureToggles
{
    public interface IFeatureTogglesServiceClient
    {
        Task<Result<IEnumerable<FeatureToggleResource>, FeatureTogglesError>> GetEnabledFeatureToggles(int entityId);
    }
}