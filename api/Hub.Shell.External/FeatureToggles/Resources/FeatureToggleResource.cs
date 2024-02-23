using System.Collections.Generic;

namespace Hub.Shell.External.FeatureToggles.Resources
{
    public class FeatureToggleResource
    {
        public int EntityId { get; set; }
        public int FeatureId { get; set; }
        public string NameKey { get; set; }
        public string Name { get; set; }
        public bool Toggle { get; set; }
        public IEnumerable<FeatureToggleResource> SubFeatures { get; set; }
    }
}