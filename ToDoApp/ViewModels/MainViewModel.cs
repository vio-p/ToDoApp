using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ToDoApp.Commands;
using ToDoApp.Models;
using ToDoApp.Services;
using ToDoApp.Views;

namespace ToDoApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public Context Context { get; set; }

        // commands
        public ICommand AddRootTDLCommand { get; }
        public ICommand AddSubTDLCommand { get; }
        public ICommand EditTDLCommand { get; }
        public ICommand DeleteTDLCommand { get; }
        public ICommand MoveUpTDLCommand { get; }
        public ICommand MoveDownTDLCommand { get; }
        public ICommand ManageCategoriesCommand { get; }

        public MainViewModel()
        {
            InitializeContext();
            
            // To Do List menu commands
            AddRootTDLCommand = new RelayCommand(ShowToDoListViewToAddRootTDL);
            AddSubTDLCommand = new RelayCommand(ShowToDoListViewToAddSubTDL, parameter => Context.IsSelectedToDoList);
            EditTDLCommand = new RelayCommand(ShowToDoListViewToEditTDL, parameter => Context.IsSelectedToDoList);
            DeleteTDLCommand = new RelayCommand(DeleteToDoList, parameter => Context.IsSelectedToDoList);
            MoveUpTDLCommand = new RelayCommand(MoveUpToDoList, parameter => Context.IsSelectedToDoList);
            MoveDownTDLCommand = new RelayCommand(MoveDownToList, parameter => Context.IsSelectedToDoList);

            // Task menu commands
            ManageCategoriesCommand = new RelayCommand(ShowManageCategoriesView);
        }

        private void InitializeContext()
        {
            // to be changed
            Context = new Context(SerializationActions.Deserialize<Database>("Test.xml"));
        }

        // command actions
        private void ShowManageCategoriesView()
        {
            ManageCategoriesView manageCategoriesView = new ManageCategoriesView(Context);
            manageCategoriesView.Show();
        }

        private void ShowToDoListViewToAddRootTDL()
        {
            Context.ActionType = EActionType.Add;
            Context.SelectedToDoList = new ToDoList();
            ToDoListView toDoListView = new ToDoListView(Context);
            toDoListView.Show();
        }

        private void ShowToDoListViewToAddSubTDL()
        {
            Context.ActionType = EActionType.Add;
            ToDoListView toDoListView = new ToDoListView(Context);
            toDoListView.Show();
        }

        private void ShowToDoListViewToEditTDL()
        {
            Context.ActionType = EActionType.Edit;
            ToDoListView toDoListView = new ToDoListView(Context);
            toDoListView.Show();
        }

        private void DeleteToDoList()
        {
            if (!(MessageBox.Show("Are you sure you want to delete the to do list?", "Please confirm.", MessageBoxButton.YesNo) == MessageBoxResult.Yes))
            {
                // cancel delete
                return;
            }
            Context.Database.DeleteToDoList(Context.SelectedToDoList);
            SerializationActions.Serialize(Context.Database, Context.Database.Name + ".xml");
        }

        private void MoveUpToDoList()
        {
            Context.Database.MoveUpToDoList(Context.SelectedToDoList);
            SerializationActions.Serialize(Context.Database, Context.Database.Name + ".xml");
        }

        private void MoveDownToList()
        {
            Context.Database.MoveDownToDoList(Context.SelectedToDoList);
            SerializationActions.Serialize(Context.Database, Context.Database.Name + ".xml");
        }

        // for event handling
        public void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Context.SelectedToDoList = e.NewValue as ToDoList;
        }
    }
}
