using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<UserMenus> UserMenus { get; set; } 
        public ICollection<UserClients> UserClients { get; set; }  // ← new
    }

}
