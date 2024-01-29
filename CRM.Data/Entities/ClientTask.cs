using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Data.Entities
{
    public class ClientTask
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("ClientId")]
        public Client? Client { get; set; }

        // Foreign key for Client
        public int ClientId { get; set; }

        // Navigation property for Tasks
        public List<Tasks> Tasks { get; set; }

    }
     
}
