using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ToDoApp.Models
{
    [Serializable]
    public class Task : ModelBase
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
        public DateTime Deadline { get; set; } = DateTime.Now;
        [XmlAttribute]
        public DateTime DateFinished { get; set; }
        [XmlAttribute]
        private bool _isCompleted;
        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                _isCompleted = value;
                DateFinished = _isCompleted ? DateTime.Now : default;
                Status = _isCompleted ? EStatus.Done : EStatus.InProgress;
            }
        }

        private Category _category;
        [XmlIgnore]
        public Category Category
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged(nameof(Category));
            }
        }

        public Task()
        {
            // empty
        }
    }
}
