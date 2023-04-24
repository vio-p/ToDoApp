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

        public void UpdateToDoList(ToDoList toDoList)
        {
            ObservableCollection<ToDoList> collection = FindParentCollection(RootToDoLists, toDoList);
            int index = collection.IndexOf(toDoList);
            _ = collection.Remove(toDoList);
            collection.Insert(index, toDoList);
        }
    }
}
