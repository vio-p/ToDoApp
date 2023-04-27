using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ToDoApp.Commands;
using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ViewModelContext Context { get; set; }

        private Visibility _statisticsVisibility = Visibility.Visible;
        public Visibility StatisticVisibility
        {
            get => _statisticsVisibility;
            set
            {
                _statisticsVisibility = value;
                OnPropertyChanged(nameof(StatisticVisibility));
            }
        }

        #region ICommandProperties
        // File menu commands
        public ICommand OpenDatabaseCommand { get; }
        public ICommand NewDatabaseCommand { get; }
        public ICommand ArchiveDatabaseCommand { get; }
        public ICommand ExitCommand { get; }

        // To Do List menu commands
        public ICommand AddRootTDLCommand { get; }
        public ICommand AddSubTDLCommand { get; }
        public ICommand EditTDLCommand { get; }
        public ICommand DeleteTDLCommand { get; }
        public ICommand MoveUpTDLCommand { get; }
        public ICommand MoveDownTDLCommand { get; }
        public ICommand ChangePathCommand { get; }

        // Task menu commands
        public ICommand AddTaskCommand { get; }
        public ICommand EditTaskCommand { get; }
        public ICommand DeleteTaskCommand { get; }
        public ICommand SetDoneTaskCommand { get; }
        public ICommand MoveUpTaskCommand { get; }
        public ICommand MoveDownTaskCommand { get; }
        public ICommand ManageCategoriesCommand { get; }
        public ICommand FindTaskCommand { get; }

        // View menu commands
        public ICommand SortByPriorityCommand { get; }
        public ICommand SortByDeadlineCommand { get; }
        public ICommand FilterByCategoryCommand { get; }
        public ICommand FilterCompletedCommand { get; }
        public ICommand FilterCompletedLateCommand { get; }
        public ICommand FilterOverdueCommand { get; }
        public ICommand FilterUncompletedWithFutureDeadlineCommand { get; }
        public ICommand StatisticsCommand { get; }

        // Help menu commands
        public ICommand AboutCommand { get; }

        #endregion

        public MainViewModel()
        {
            InitializeContext();

            #region ICommandPropertyInitializations
            // File menu commands
            OpenDatabaseCommand = new RelayCommand(Context.OpenDatabase);
            NewDatabaseCommand = new RelayCommand(Context.CreateDatabase);
            ArchiveDatabaseCommand = new RelayCommand(Context.ArchiveDatabase, parameter => Context.Database != null);
            ExitCommand = new RelayCommand(Exit);
            
            // To Do List menu commands
            AddRootTDLCommand = new RelayCommand(Context.AddRootToDoList, parameter => Context.Database != null);
            AddSubTDLCommand = new RelayCommand(Context.AddSubToDoList, parameter => Context.SelectedToDoList != null);
            EditTDLCommand = new RelayCommand(Context.EditSelectedToDoList, parameter => Context.SelectedToDoList != null);
            DeleteTDLCommand = new RelayCommand(Context.DeleteSelectedToDoList, parameter => Context.SelectedToDoList != null);
            MoveUpTDLCommand = new RelayCommand(Context.MoveUpSelectedToDoList, parameter => Context.SelectedToDoList != null);
            MoveDownTDLCommand = new RelayCommand(Context.MoveDownSelectedToDoList, parameter => Context.SelectedToDoList != null);
            ChangePathCommand = new RelayCommand(Context.ChangePathOfSelectedToDoList, parameter => Context.SelectedToDoList != null);

            // Task menu commands
            AddTaskCommand = new RelayCommand(Context.AddTask, parameter => Context.SelectedToDoList != null);
            EditTaskCommand = new RelayCommand(Context.EditSelectedTask, parameter => Context.SelectedTask != null);
            DeleteTaskCommand = new RelayCommand(Context.DeleteSelectedTask, parameter => Context.SelectedTask != null);
            SetDoneTaskCommand = new RelayCommand(Context.SetDoneSelectedTask, parameter => Context.SelectedTask != null && !Context.SelectedTask.IsCompleted);
            MoveUpTaskCommand = new RelayCommand(Context.MoveUpSelectedTask, parameter => Context.SelectedTask != null);
            MoveDownTaskCommand = new RelayCommand(Context.MoveDownSelectedTask, parameter => Context.SelectedTask != null);
            ManageCategoriesCommand = new RelayCommand(Context.ShowManageCategoriesView, parameter => Context.Database != null);
            FindTaskCommand = new RelayCommand(Context.ShowFindTaskView, parameter => Context.Database != null);

            // View menu commands
            SortByPriorityCommand = new RelayCommand(Context.SortByPriority, parameter => Context.Database != null);
            SortByDeadlineCommand = new RelayCommand(Context.SortByDeadline, parameter => Context.Database != null);
            FilterByCategoryCommand = new RelayCommand(Context.FilterByCategory, parameter => Context.Database != null);
            FilterCompletedCommand = new RelayCommand(Context.FilterCompleted, parameter => Context.Database != null);
            FilterCompletedLateCommand = new RelayCommand(Context.FilterCompletedLate, parameter => Context.Database != null);
            FilterOverdueCommand = new RelayCommand(Context.FilterOverdue, parameter => Context.Database != null);
            FilterUncompletedWithFutureDeadlineCommand = new RelayCommand(Context.FilterUncompletedWithFutureDeadline, parameter => Context.Database != null);
            StatisticsCommand = new RelayCommand(ModifyStatisticsVisibility); // should also be conditioned by having an open database

            // Help menu commands
            AboutCommand = new RelayCommand(ShowAbout);

            #endregion
        }

        private void InitializeContext()
        {
            Context = new ViewModelContext();

            if (!Directory.Exists("Databases"))
            {
                _ = Directory.CreateDirectory("Databases");
                _ = Directory.CreateDirectory(@"Databases\Archived");
                _ = File.Create(@"Databases\last_opened.txt");
            }

            string lastOpenedPath = File.ReadAllText(@"Databases\last_opened.txt");
            if (lastOpenedPath != "")
            {
                Context.Database = SerializationService.Deserialize<Database>(lastOpenedPath);
            }
        }

        // command actions
        private void Exit()
        {
            File.WriteAllText(@"Databases\last_opened.txt", Context.Database.Path);
            Application.Current.Shutdown();
        }

        private void ModifyStatisticsVisibility()
        {
            StatisticVisibility = StatisticVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void ShowAbout()
        {
            _ = MessageBox.Show("Pușcaș Viorica" + Environment.NewLine + "10LF313" + Environment.NewLine + "viorica.puscas@student.unitbv.ro", "About", MessageBoxButton.OK);
        }

        // for event handling
        public void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Context.SelectedToDoList = e.NewValue as ToDoList;
            Context.SelectedTask = null;
        }

        public void TasksDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Context.SelectedTask = (sender as DataGrid).SelectedItem as Task;
        }
    }
}
