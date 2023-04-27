using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ToDoApp.Models
{
    [Serializable]
    public class ToDoList : ModelBase
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

        private string _iconPath;
        [XmlAttribute]
        public string IconPath
        {
            get => _iconPath;
            set
            {
                _iconPath = value;
                OnPropertyChanged(nameof(IconPath));
            }
        }

        [XmlAttribute]
        public string ParentName { get; set; }
        [XmlArray]
        public ObservableCollection<ToDoList> ToDoLists { get; set; }
        [XmlArray]
        public ObservableCollection<Task> Tasks { get; set; }
        
        public ToDoList()
        {
            ToDoLists = new ObservableCollection<ToDoList>();
            Tasks = new ObservableCollection<Task>();
        }

        public void MoveUpTask(Task task)
        {
            int index = Tasks.IndexOf(task);
            if (index > 0)
            {
                Tasks.Move(index, index - 1);
            }
        }

        public void MoveDownTask(Task task)
        {
            int index = Tasks.IndexOf(task);
            if (index < Tasks.Count - 1)
            {
                Tasks.Move(index, index + 1);
            }
        }

        private bool FindChild(ToDoList parentToDoList, ToDoList childToDoList)
        {
            if (parentToDoList == childToDoList)
            {
                return true;
            }
            foreach (ToDoList tdl in parentToDoList.ToDoLists)
            {
                if (tdl == childToDoList)
                {
                    return true;
                }
                bool found = FindChild(tdl, childToDoList);
                if (found)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsOrIsChildOf(ToDoList toDoList)
        {
            return FindChild(toDoList, this);
        }
    }
}
