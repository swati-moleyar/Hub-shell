namespace Hub.Shell.External.UserManager.Resources
{
    public class AssignedSecurityRoleResource
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SecurityRoleId { get; set; }
        public int EntityId { get; set; }
    }
}