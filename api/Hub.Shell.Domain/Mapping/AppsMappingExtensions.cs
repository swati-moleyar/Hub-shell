using System.Collections.Generic;
using System.Linq;
using Hub.Shell.Contracts;
using Hub.Shell.External.Hub.Resources;

namespace Hub.Shell.Domain.Mapping
{
    public static class AppsMappingExtensions
    {
        public static ApplicationContract ToContract(this ApplicationResource resource)
        {
            return new ApplicationContract 
            {
                Id = resource.Id,
                Name = resource.Title,
                Description = resource.Description,
                Href = resource.Href,
                Version = resource.Version.HasValue ? resource.Version.Value : 1
            };
        }

        public static ApplicationGroupContract ToContract(this ApplicationGroupResource resource, IEnumerable<ApplicationResource> apps, string origin)
        {
            var groupName = resource.DomainRebrand?.FirstOrDefault(x => origin?.Contains(x.Key) ?? false).Value ?? resource.GroupName;
            var defaultApp = resource.DisableToggle ? apps.FirstOrDefault(app => app.Id == resource.DefaultApp)?.ToContract() : null;
            var groupApps = resource.Applications
                .Select(appId => apps.FirstOrDefault(app => app.Id == appId)?.ToContract())
                .Where(app => app != null)
                .OrderBy(app => resource.UseDefaultOrder ? "" : app.Name);

            return groupApps.Any() 
                ? new ApplicationGroupContract 
                {
                    Name = groupName,
                    Icon = resource.Icon,
                    DefaultApp = defaultApp,
                    Apps = groupApps
                }
                : null;
        }
    }
}