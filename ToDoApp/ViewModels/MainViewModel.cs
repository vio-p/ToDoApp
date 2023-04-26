using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        #region Commands
        // To Do List menu commands
        public ICommand AddRootTDLCommand { get; }
        public ICommand AddSubTDLCommand { get; }
        public ICommand EditTDLCommand { get; }
        public ICommand DeleteTDLCommand { get; }
        public ICommand MoveUpTDLCommand { get; }
        public ICommand MoveDownTDLCommand { get; }

        // Task menu commands
        public ICommand AddTaskCommand { get; }
        public ICommand EditTaskCommand { get; }
        public ICommand DeleteTaskCommand { get; }
        public ICommand SetDoneTaskCommand { get; }
        public ICommand MoveUpTaskCommand { get; }
        public ICommand MoveDownTaskCommand { get; }
        public ICommand ManageCategoriesCommand { get; }

        #endregion

        public MainViewModel()
        {
            InitializeContext();
            
            // To Do List menu commands
            AddRootTDLCommand = new RelayCommand(Context.AddRootToDoList);
            AddSubTDLCommand = new RelayCommand(Context.AddSubToDoList, parameter => Context.SelectedToDoList != null);
            EditTDLCommand = new RelayCommand(Context.EditSelectedToDoList, parameter => Context.SelectedToDoList != null);
            DeleteTDLCommand = new RelayCommand(Context.DeleteSelectedToDoList, parameter => Context.SelectedToDoList != null);
            MoveUpTDLCommand = new RelayCommand(Context.MoveUpSelectedToDoList, parameter => Context.SelectedToDoList != null);
            MoveDownTDLCommand = new RelayCommand(Context.MoveDownSelectedToDoList, parameter => Context.SelectedToDoList != null);

            // Task menu commands
            AddTaskCommand = new RelayCommand(Context.AddTask, parameter => Context.SelectedToDoList != null);
            EditTaskCommand = new RelayCommand(Context.EditSelectedTask, parameter => Context.SelectedTask != null);
            DeleteTaskCommand = new RelayCommand(Context.DeleteSelectedTask, parameter => Context.SelectedTask != null);
            SetDoneTaskCommand = new RelayCommand(Context.SetDoneSelectedTask, parameter => Context.SelectedTask != null && !Context.SelectedTask.IsCompleted);
            MoveUpTaskCommand = new RelayCommand(Context.MoveUpSelectedTask, parameter => Context.SelectedTask != null);
            MoveDownTaskCommand = new RelayCommand(Context.MoveDownSelectedTask, parameter => Context.SelectedTask != null);
            ManageCategoriesCommand = new RelayCommand(Context.ShowManageCategoriesView);
        }

        private void InitializeContext()
        {
            // to be changed
            Context = new ViewModelContext(SerializationService.Deserialize<Database>("Test.xml"));
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
