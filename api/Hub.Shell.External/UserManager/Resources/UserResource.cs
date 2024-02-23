namespace Hub.Shell.External.UserManager.Resources
{
    public class UserResource
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string ParentEntityName { get; set; }
        public int ParentEntityId { get; set; }
    }
}