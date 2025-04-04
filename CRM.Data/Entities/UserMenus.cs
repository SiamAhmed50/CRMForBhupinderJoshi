using CRM.Data.DbContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Data.Entities
{
    public class UserMenus
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int MenuId { get; set; }
        public Menus Menu { get; set; }
    }


}
