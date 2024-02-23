using System.Collections.Generic;

namespace Hub.Shell.Contracts
{
    public class SessionContract
    {
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public bool CanChangePassword { get; set; }
        public string BrandingLogo { get; set; }
        public string ParentEntityRole { get; set; }
        public IEnumerable<ApplicationGroupContract> ApplicationGroups { get; set; }
    }
}