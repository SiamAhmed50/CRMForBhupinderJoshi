namespace CRM.UI.ViewModel
{
    public class RegisterUserDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public List<int> MenuIds { get; set; }
        public List<int> ClientIds { get; set; }    // ← new
    }


}
