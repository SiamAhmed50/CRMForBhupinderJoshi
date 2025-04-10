namespace CRM.UI.ViewModel
{
    public class UserListDto
    {
        
            public string Name { get; set; }
            public string Email { get; set; }
            public List<int> MenuIds { get; set; }
            public List<string> MenuNames { get; set; } // <-- Add this
        
        // Comma-separated list
    }

}
