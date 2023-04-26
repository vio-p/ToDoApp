using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ToDoApp.Models
{
    [Serializable]
    public class Task : ModelBase
    {
        private string _name;
        [XmlAttribute]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _description;
        [XmlAttribute]
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        [XmlAttribute]
        public int CategoryId { get; set; }

        private EStatus _status;
        [XmlElement]
        public EStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        private EPriority _priority;
        [XmlElement]
        public EPriority Priority
        {
            get => _priority;
            set
            {
                _priority = value;
                OnPropertyChanged(nameof(Priority));
            }
        }

        private DateTime _deadline = DateTime.Now;
        [XmlAttribute]
        public DateTime Deadline
        {
            get => _deadline;
            set
            {
                _deadline = value;
                OnPropertyChanged(nameof(Deadline));
            }
        }

        [XmlAttribute]
        public DateTime DateFinished { get; set; }
        
        private bool _isCompleted;
        [XmlAttribute]
        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                DateFinished = _isCompleted ? default : DateTime.Now;
                Status = _isCompleted ? EStatus.InProgress : EStatus.Done;
                _isCompleted = value;
                OnPropertyChanged(nameof(IsCompleted));
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
                if (_category != null)
                {
                    CategoryId = _category.Id;
                }
                OnPropertyChanged(nameof(Category));
            }
        }

        public Task()
        {
            // empty
        }

        public bool IsCompletedLate()
        {
            return IsCompleted && DateFinished.Date > Deadline.Date;
        }

        public bool IsOverdue()
        {
            return !IsCompleted && Deadline.Date < DateTime.Now.Date;
        }

        public bool IsUncompletedWithFutureDeadline()
        {
            return !IsCompleted && Deadline.Date > DateTime.Now.Date;
        }
    }
}
