using System.Collections.Generic;

namespace Hub.Shell.External.UserManager.Resources
{
    public class UserManagerError
    {
        public string Message { get; set; }
        public string Description { get; set; }
        public IEnumerable<string> Details { get; set; }
    }
}