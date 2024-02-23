using System.Collections.Generic;

namespace Hub.Shell.External.Auth.Resources
{
    public class AuthError
    {
        public string Message { get; set; }
        public string Description { get; set; }
        public IEnumerable<string> Details { get; set; }
    }
}