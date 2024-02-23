using System.Collections.Generic;

namespace Hub.Shell.External.Hub.Resources
{
    public class ApplicationGroupResource
    {
        public string GroupName { get; set; }
        public IDictionary<string, string> DomainRebrand { get; set; }
        public string[] Applications { get; set; }
        public string DefaultApp { get; set; }
        public string Icon { get; set; }
        public string Path { get; set; }
        public bool DisableToggle { get; set; }
        public bool UseDefaultOrder { get; set; }
    }
}