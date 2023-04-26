using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using ToDoApp.Models;
using ToDoApp.Services;
using ToDoApp.Views;

namespace ToDoApp.ViewModels
{
    public class ViewModelContext : ViewModelBase
    {
        public Database Database { get; set; }
        public EActionType ActionType { get; set; }
        public Window OpenWindow { get; set; }

        private ToDoList _selectedToDoList;
        public ToDoList SelectedToDoList
        {
            get => _selectedToDoList;
            set
            {
                _selectedToDoList = value;
                OnPropertyChanged(nameof(SelectedToDoList));
            }
        }

        private Task _selectedTask;
        public Task SelectedTask
        {
            get => _selectedTask;
            set
            {
                _selectedTask = value;
                OnPropertyChanged(nameof(SelectedTask));
            }
        }

        public ViewModelContext(Database database)
        {
            Database = database;
            Database.Initialize();
        }

        public void SaveDatabase()
        {
            SerializationService.Serialize(Database, Database.Name + ".xml");
        }

        public void AddRootToDoList()
        {
            ActionType = EActionType.Add;
            SelectedToDoList = null; // so that the view model knows a root to do list should be added
            ToDoListView toDoListView = new ToDoListView(this);
            toDoListView.Show();
        }

        public void AddSubToDoList()
        {
            ActionType = EActionType.Add;
            ToDoListView toDoListView = new ToDoListView(this);
            toDoListView.Show();
        }

        public void EditSelectedToDoList()
        {
            ActionType = EActionType.Edit;
            ToDoListView toDoListView = new ToDoListView(this);
            OpenWindow = toDoListView;
            toDoListView.Show();
        }

        public void DeleteSelectedToDoList()
        {
            if (!(MessageBox.Show("Are you sure you want to delete the to do list?", "Please confirm.", MessageBoxButton.YesNo) == MessageBoxResult.Yes))
            {
                // cancel delete
                return;
            }
            Database.DeleteToDoList(SelectedToDoList);
            SelectedToDoList = null;
            SaveDatabase();
        }

        public void MoveUpSelectedToDoList()
        {
            Database.MoveUpToDoList(SelectedToDoList);
            SaveDatabase();
        }

        public void MoveDownSelectedToDoList()
        {
            Database.MoveDownToDoList(SelectedToDoList);
            SaveDatabase();
        }

        public void AddTask()
        {
            ActionType = EActionType.Add;
            TaskView taskView = new TaskView(this);
            taskView.Show();
        }

        public void EditSelectedTask()
        {
            ActionType = EActionType.Edit;
            TaskView taskView = new TaskView(this);
            OpenWindow = taskView;
            taskView.Show();
        }

        public void DeleteSelectedTask()
        {
            if (!(MessageBox.Show("Are you sure you want to delete the task?", "Please confirm.", MessageBoxButton.YesNo) == MessageBoxResult.Yes))
            {
                // cancel delete
                return;
            }
            _ = SelectedToDoList.Tasks.Remove(SelectedTask);
            SelectedTask = null;
            SaveDatabase();
        }

        public void SetDoneSelectedTask()
        {
            SelectedTask.IsCompleted = true;
            SelectedToDoList.UpdateTask(SelectedTask);
            SaveDatabase();
        }

        public void MoveUpSelectedTask()
        {
            SelectedToDoList.MoveUpTask(SelectedTask);
            SaveDatabase();
        }

        public void MoveDownSelectedTask()
        {
            SelectedToDoList.MoveDownTask(SelectedTask);
            SaveDatabase();
        }
        public void ShowManageCategoriesView()
        {
            ManageCategoriesView manageCategoriesView = new ManageCategoriesView(this);
            manageCategoriesView.Show();
        }
    }
}
