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
        public Database Database { get; }
        public EActionType ActionType { get; set; }
        public Window OpenWindow { get; set; }

        private ObservableCollection<Task> _displayedTasks;
        public ObservableCollection<Task> DisplayedTasks
        {
            get => _displayedTasks;
            set
            {
                _displayedTasks = value;
                OnPropertyChanged(nameof(DisplayedTasks));
            }
        }

        private ToDoList _selectedToDoList;
        public ToDoList SelectedToDoList
        {
            get => _selectedToDoList;
            set
            {
                _selectedToDoList = value;
                DisplayedTasks = _selectedToDoList != null ? _selectedToDoList.Tasks : null;
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

        public Category CategoryToFilterBy { get; set; }

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
            SaveDatabase();
        }

        public void SetDoneSelectedTask()
        {
            SelectedTask.IsCompleted = true;
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

        public void ShowFindTaskView()
        {
            FindTaskView findTaskView = new FindTaskView(this);
            findTaskView.Show();
        }

        public void SortByPriority()
        {
            List<Task> tasks = SelectedToDoList.Tasks.ToList();
            tasks.Sort(new Comparison<Task>((x, y) => x.Priority.CompareTo(y.Priority)));
            DisplayedTasks = new ObservableCollection<Task>(tasks);
        }

        public void SortByDeadline()
        {
            List<Task> tasks = SelectedToDoList.Tasks.ToList();
            tasks.Sort(new Comparison<Task>((x, y) => x.Deadline.CompareTo(y.Deadline)));
            DisplayedTasks = new ObservableCollection<Task>(tasks);
        }

        public void FilterByCategory()
        {
            SelectCategoryView selectCategoryView = new SelectCategoryView(this);
            OpenWindow = selectCategoryView;
            selectCategoryView.Show();
        }

        public void FilterCompleted()
        {
            DisplayedTasks = new ObservableCollection<Task>(SelectedToDoList.Tasks.Where(task => task.IsCompleted));
        }

        public void FilterCompletedLate()
        {
            DisplayedTasks = new ObservableCollection<Task>(SelectedToDoList.Tasks.Where(task => task.IsCompletedLate()));
        }

        public void FilterOverdue()
        {
            DisplayedTasks = new ObservableCollection<Task>(SelectedToDoList.Tasks.Where(task => task.IsOverdue()));
        }

        public void FilterUncompletedWithFutureDeadline()
        {
            DisplayedTasks = new ObservableCollection<Task>(SelectedToDoList.Tasks.Where(task => task.IsUncompletedWithFutureDeadline()));
        }
    }
}
