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
    public class Database
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public DateTime DateCreated { get; set; }
        [XmlArray]
        public ObservableCollection<ToDoList> RootToDoLists { get; set; }
        [XmlArray]
        public ObservableCollection<Category> Categories { get; set; }

        private List<Task> _allTasks;

        public Database()
        {
            // empty
        }

        public Database(string name)
        {
            Name = name;
            DateCreated = DateTime.Now;
            RootToDoLists = new ObservableCollection<ToDoList>();
            Categories = new ObservableCollection<Category>();
        }

        private void AddTasks(ObservableCollection<ToDoList> collection)
        {
            if (collection.Count == 0)
            {
                return;
            }
            foreach (ToDoList toDoList in collection)
            {
                _allTasks.AddRange(toDoList.Tasks);
                AddTasks(toDoList.ToDoLists);
            }
        }

        public List<Task> GetAllTasks()
        {
            _allTasks = new List<Task>();
            AddTasks(RootToDoLists);
            return _allTasks;
        }

        private void SetTaskCategories(ObservableCollection<ToDoList> collection)
        {
            if (collection.Count == 0)
            {
                return;
            }
            foreach (ToDoList tdl in collection)
            {
                foreach (Task task in tdl.Tasks)
                {
                    if (task.CategoryId > 0)
                    {
                        task.Category = Categories.Single(category => category.Id == task.CategoryId);
                    }
                }
                SetTaskCategories(tdl.ToDoLists);
            }
        }

        public void Initialize()
        {
            SetTaskCategories(RootToDoLists);
        }

        private ObservableCollection<ToDoList> FindParentCollection(ObservableCollection<ToDoList> collection, ToDoList toDoList)
        {
            if (collection.Contains(toDoList))
            {
                return collection;
            }
            foreach (ToDoList tdl in collection)
            {
                ObservableCollection<ToDoList> parent = FindParentCollection(tdl.ToDoLists, toDoList);
                if (parent != null)
                {
                    return parent;
                }
            }
            return null;
        }

        public void DeleteToDoList(ToDoList toDoList)
        {
            _ = FindParentCollection(RootToDoLists, toDoList).Remove(toDoList);
        }

        public void MoveUpToDoList(ToDoList toDoList)
        {
            ObservableCollection<ToDoList> collection = FindParentCollection(RootToDoLists, toDoList);
            int index = collection.IndexOf(toDoList);
            if (index > 0)
            {
                collection.Move(index, index - 1);
            }
        }

        public void MoveDownToDoList(ToDoList toDoList)
        {
            ObservableCollection<ToDoList> collection = FindParentCollection(RootToDoLists, toDoList);
            int index = collection.IndexOf(toDoList);
            if (index < collection.Count - 1)
            {
                collection.Move(index, index + 1);
            }
        }
    }
}
