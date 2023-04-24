using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ToDoApp.Models
{
    [Serializable]
    public class Task
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string Description { get; set; }
        [XmlAttribute]
        public int CategoryId { get; set; }
        [XmlElement]
        public EStatus Status { get; set; }
        [XmlElement]
        public EPriority Priority { get; set; }
        [XmlAttribute]
        public DateTime Deadline { get; set; }
        [XmlAttribute]
        public DateTime DateFinished { get; set; }
        [XmlAttribute]
        bool IsFinished { get; set; }

        public Task()
        {
            // empty
        }

        public Task(string name, string description, int categoryId, EStatus status, EPriority priority, DateTime deadline, DateTime dateFinished)
        {
            Name = name;
            Description = description;
            CategoryId = categoryId;
            Status = status;
            Priority = priority;
            Deadline = deadline;
            DateFinished = dateFinished;
            IsFinished = false;
        }
    }
}
