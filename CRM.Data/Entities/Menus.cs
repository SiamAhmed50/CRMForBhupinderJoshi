using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Data.Entities
{
    public class Menus
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }           // e.g., Dashboard, Users, Settings
        public string Status { get; set; }         // e.g., Active, Inactive

        // Optional (for hierarchy or navigation)
        public string Url { get; set; }            // e.g., "/dashboard"
        public int? ParentId { get; set; }
    }

}
