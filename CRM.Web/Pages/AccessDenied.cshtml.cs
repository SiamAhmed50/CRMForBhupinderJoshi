using CRM.UI.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CRM.UI.Pages
{
    public class AccessDeniedModel : PageModel
    {
        [MenuAuthorize("/Clients")]
        public class ClientsModel : PageModel
        {
            public void OnGet() { }
        }
    }
}
