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
    public class ToDoList
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string IconPath { get; set; }
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

        public void UpdateTask(Task task)
        {
            int index = Tasks.IndexOf(task);
            _ = Tasks.Remove(task);
            Tasks.Insert(index, task);
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
    }
}
