using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Functional;
using Hub.Shell.Error;
using Hub.Shell.External.FeatureToggles;
using Hub.Shell.External.FeatureToggles.Resources;
using Hub.Shell.External.Hub.Resources;

namespace Hub.Shell.Domain.Factories.Filters
{
    public class ApplicationsFeaturesFilter : IApplicationsFeaturesFilter
    {
        private readonly IFeatureTogglesServiceClient _featureToggles;

        public ApplicationsFeaturesFilter(IFeatureTogglesServiceClient featureToggles)
        {
            _featureToggles = featureToggles;
        }

        public Task<Result<IEnumerable<ApplicationResource>, ServiceError>> Apply(IEnumerable<ApplicationResource> apps, int entityId)
        {
            return GetFeatureToggles(entityId)
                .Map(featureToggles => FlattenFeatureToggles(featureToggles))
                .Map(featureKeys => apps.Where(app => HasFeatureToggles(app, featureKeys)));
        }

        private Task<Result<IEnumerable<FeatureToggleResource>, ServiceError>> GetFeatureToggles(int entityId)
        {
            return _featureToggles.GetEnabledFeatureToggles(entityId).Match(
                featureToggles => Result.Success<IEnumerable<FeatureToggleResource>, ServiceError>(featureToggles),
                featureTogglesError => Result.Failure<IEnumerable<FeatureToggleResource>, ServiceError>(new ServiceError(ErrorType.BadRequestData, featureTogglesError.Message))
            );
        }

        private HashSet<string> FlattenFeatureToggles(IEnumerable<FeatureToggleResource> toggles)
        {
            if (toggles == null || !toggles.Any())
            {
                return new HashSet<string>();
            }
            return toggles.SelectMany(x => FlattenFeatureToggles(x.SubFeatures).Append(x.NameKey)).ToHashSet();
        }

        private bool HasFeatureToggles(ApplicationResource app, HashSet<string> featureToggles)
        {
            if (app.FeatureToggles != null)
            {
                return app.FeatureToggles.All(toggle => featureToggles.Contains(toggle));
            }
            return true;
        }
    }
}