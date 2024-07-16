using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CRM.Data.Entities
{
    public class Tasks
    {
        public Tasks()
        {
            GenerateUnique4DigitId();
        }
        [Key]
        public int Id { get; set; }
        public int TaskId { get; set; }

        public string Name { get; set; }

        [ForeignKey(nameof(ClientTask))]
        public int? ClientTaskId { get; set; }
        [JsonIgnore]
        public ClientTask? ClientTask { get; set; }

        // Foreign key for Client
        

        // Custom logic to generate unique 4-digit IDs
        public void GenerateUnique4DigitId()
        {
            Random random = new Random();
            HashSet<int> existingIds = new HashSet<int>();

            // Ensure that the generated ID is unique
            do
            {
                TaskId = random.Next(1000, 10000);
            } while (!existingIds.Add(Id));
        }
    }


}
