using CRM.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Data.Entities
{
    public class ClientTask
    {
        [Key]
        public int Id { get; set; }
        public Client? Client { get; set; }

        // Foreign key for Clienta
        public int ClientId { get; set; }
        public ClientTaskStatus Status { get; set; }

        // Navigation property for Tasks
        public List<Tasks> Tasks { get; set; }

    }
     
}
