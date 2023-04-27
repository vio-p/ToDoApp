using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
        private Database _database;
        public Database Database
        {
            get => _database;
            set
            {
                _database = value;
                if (_database != null)
                {
                    DueToday = _database.GetAllTasks().Where(task => task.IsDueToday()).ToList().Count;
                    DueTomorrow = _database.GetAllTasks().Where(task => task.IsDueTomorrow()).ToList().Count;
                    Overdue = _database.GetAllTasks().Where(task => task.IsOverdue()).ToList().Count;
                    Done = _database.GetAllTasks().Where(task => task.IsCompleted).ToList().Count;
                    ToBeDone = _database.GetAllTasks().Where(task => !task.IsCompleted).ToList().Count;
                    _database.Initialize();
                }
                OnPropertyChanged(nameof(Database));
            }
        }

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

        private int _dueToday;
        private int _dueTomorrow;
        private int _overdue;
        private int _done;
        private int _toBeDone;

        public int DueToday
        {
            get => _dueToday;
            set
            {
                _dueToday = value;
                LabelDueToday = "Tasks due today: " + _dueToday;
            }
        }
        public int DueTomorrow
        {
            get => _dueTomorrow;
            set
            {
                _dueTomorrow = value;
                LabelDueTomorrow = "Tasks due tomorrow: " + _dueTomorrow;
            }
        }
        public int Overdue
        {
            get => _overdue;
            set
            {
                _overdue = value;
                LabelOverdue = "Tasks overdue: " + _overdue;
            }
        }
        public int Done
        {
            get => _done; set
            {
                _done = value;
                LabelDone = "Tasks done: " + _done;
            }
        }
        public int ToBeDone
        {
            get => _toBeDone;
            set
            {
                _toBeDone = value;
                LabelToBeDone = "Tasks to be done: " + _toBeDone;
            }
        }

        private string _labelDueToday = "";
        private string _labelDueTomorrow = "";
        private string _labelOverdue = "";
        private string _labelDone = "";
        private string _labelToBeDone = "";

        public string LabelDueToday
        {
            get => _labelDueToday;
            set
            {
                _labelDueToday = value;
                OnPropertyChanged(nameof(LabelDueToday));
            }
        }

        public string LabelDueTomorrow
        {
            get => _labelDueTomorrow;
            set
            {
                _labelDueTomorrow = value;
                OnPropertyChanged(nameof(LabelDueTomorrow));
            }
        }
        
        public string LabelOverdue
        {
            get => _labelOverdue;
            set
            {
                _labelOverdue = value;
                OnPropertyChanged(nameof(LabelOverdue));
            }
        }

        public string LabelDone
        {
            get => _labelDone;
            set
            {
                _labelDone = value;
                OnPropertyChanged(nameof(LabelDone));
            }
        }

        public string LabelToBeDone
        {
            get => _labelToBeDone;
            set
            {
                _labelToBeDone = value;
                OnPropertyChanged(nameof(LabelToBeDone));
            }
        }

        public ViewModelContext()
        {
            // empty
        }

        public void SaveDatabase()
        {
            SerializationService.Serialize(Database, Database.Path);
        }

        public void CreateDatabase()
        {
            CreateDatabaseView createDatabaseView = new CreateDatabaseView();
            createDatabaseView.Show();
        }

        public void OpenDatabase()
        {
            OpenDatabaseView openDatabaseView = new OpenDatabaseView(this);
            OpenWindow = openDatabaseView;
            openDatabaseView.Show();
        }

        public void ArchiveDatabase()
        {
            string archivedDirectoryPath = @"Databases\Archived";

            File.Delete(Database.Path);
            Database.Path = archivedDirectoryPath + @"\" + Database.Name + ".xml";
            SerializationService.Serialize(Database, Database.Path);

            File.WriteAllText(@"Databases\last_opened.txt", "");

            Database = null;
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

        public void ChangePathOfSelectedToDoList()
        {
            ChangePathView changePathView = new ChangePathView(this);
            OpenWindow = changePathView;
            changePathView.Show();
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
            Done++;
            ToBeDone--;
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
