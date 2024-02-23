using System.Collections.Generic;

namespace Hub.Shell.External.Hub.Resources
{
    public class ApplicationsResource
    {
        public ApplicationGroupResource[] ApplicationGroups { get; set; }
        public IDictionary<string, ApplicationGroupResource[]> ApplicationGroupVariations { get; set; }
        public IDictionary<string, ApplicationResource> Applications { get; set; }
    }
}