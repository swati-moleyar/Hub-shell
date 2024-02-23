namespace Hub.Shell.External.UserManager.Resources
{
    public class PermissionResource
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int? ParentPermissionId { get; set; }
    }
}