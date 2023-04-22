using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp.Models
{
    [Serializable]
    public class Task
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public EStatus Status { get; set; }
        public EPriority Priority { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime DateFinished { get; set; }
        
    }
}
