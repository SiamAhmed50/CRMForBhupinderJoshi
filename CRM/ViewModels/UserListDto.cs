namespace CRM.API.ViewModels
{
    public class UserListDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public List<int> MenuIds { get; set; }
    }

}
