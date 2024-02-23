namespace Hub.Shell.External.Hub.Resources
{
    public class ApplicationResource
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Href { get; set; }
        public string Icon { get; set; }
        public string[] Permissions { get; set; }
        public string AssertionType { get; set; }
        public string[] FeatureToggles { get; set; }
        public string EnvironmentToggleLevel { get; set; }
        public int? Version { get; set; }
    }
}