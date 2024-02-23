using System.Collections.Generic;

namespace Hub.Shell.Contracts
{
    public class ApplicationGroupContract
    {
        /// <summary>
        /// Name of the group to be displayed
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The icon classes for the group, usually from Font Awesome
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// Application to be launched when the group is selected, instead of expanding the group
        /// </summary>
        public ApplicationContract DefaultApp { get; set; }
        /// <summary>
        /// List of applications to be displayed when the group is expanded
        /// </summary>
        public IEnumerable<ApplicationContract> Apps { get; set; }
    }

    public class ApplicationContract
    {
        /// <summary>
        /// Unique, case-sensitive ID for this application
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Name of the application to be displayed
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// A brief summary of the application
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// The relative path to the application
        /// <summary>
        public string Href { get; set; }
        /// <summary>
        /// The version of Hub the application is built in
        /// </summary>
        public int Version { get; set; }
    }
}