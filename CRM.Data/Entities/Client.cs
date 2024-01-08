using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Data.Entities
{
    public class Client
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int ClientId { get; set; }
        public string LicenseNumber { get; set; }
        public DateTime LicenseStartDate { get; set; }
        public DateTime LicenseEndDate { get; set; }
    }
}
