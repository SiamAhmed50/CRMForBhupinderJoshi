using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Data.Entities
{
    public class Machine
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Client")]
        public int ClientId { get; set; }

        public string MachineIp { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        // Navigation Property
        public Client Client { get; set; }
    }
}
