using System.Collections.Generic;

namespace Hub.Shell.External.EntityStore.Resources
{
    public class EntityStoreError
    {
        public string Message { get; set; }
        public string Description { get; set; }
        public IEnumerable<string> Details { get; set; }
    }
}