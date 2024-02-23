using System.Collections.Generic;

namespace Hub.Shell.External.FeatureToggles.Resources
{
    public class FeatureTogglesError
    {
        public string Message { get; set; }
        public string Description { get; set; }
        public IEnumerable<string> Details { get; set; }
    }
}