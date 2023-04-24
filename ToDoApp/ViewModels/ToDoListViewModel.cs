using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ToDoApp.Commands;
using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp.ViewModels
{
    public class ToDoListViewModel : ViewModelBase
    {
        public Context Context { get; set; }
        public List<string> IconPaths { get; set; }
        public bool CanExecuteAction { get; set; } = false;

        private readonly ToDoList _newToDoList;
        public string Name
        {
            get => _newToDoList.Name;
            set
            {
                _newToDoList.Name = value;
                CanExecuteAction = _newToDoList.Name != "" && _newToDoList.IconPath != null;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string IconPath
        {
            get => _newToDoList.IconPath;
            set
            {
                _newToDoList.IconPath = value;
                CanExecuteAction = _newToDoList.Name != "" && _newToDoList.IconPath != null;
            }
        }

        public string ParentName { get; }

        public ICommand ActionButtonCommand { get; }

        public ToDoListViewModel(Context context)
        {
            Context = context;
            IconPaths = new List<string>(Directory.GetFiles(Environment.CurrentDirectory + @"..\..\..\Resources\Icons", "*.png", SearchOption.TopDirectoryOnly));
            _newToDoList = new ToDoList();

            if (Context.ActionType == EActionType.Add)
            {
                if (Context.SelectedToDoList.Name == null) // adding root to do list
                {
                    ActionButtonCommand = new RelayCommand(AddRootToDoList, parameter => CanExecuteAction);
                    ParentName = "";
                }
                else // adding sub to do list
                {
                    ActionButtonCommand = new RelayCommand(AddSubToDoList, parameter => CanExecuteAction);
                    ParentName = Context.SelectedToDoList.Name;
                }
            }
            if (Context.ActionType == EActionType.Edit)
            {
                ActionButtonCommand = new RelayCommand(EditToDoList, parameter => CanExecuteAction);

                Name = Context.SelectedToDoList.Name;
                IconPath = Context.SelectedToDoList.IconPath;
                ParentName = Context.SelectedToDoList.ParentName;
            }
        }

        // command actions
        private void AddRootToDoList()
        {
            _newToDoList.ParentName = ParentName;
            Context.Database.RootToDoLists.Add(_newToDoList);
            SerializationActions.Serialize(Context.Database, Context.Database.Name + ".xml");
            _ = MessageBox.Show("The to do list has been added!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AddSubToDoList()
        {
            _newToDoList.ParentName = ParentName;
            Context.SelectedToDoList.ToDoLists.Add(_newToDoList);
            SerializationActions.Serialize(Context.Database, Context.Database.Name + ".xml");
            _ = MessageBox.Show("The to do list has been added!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void EditToDoList()
        {
            Context.SelectedToDoList.Name = _newToDoList.Name;
            Context.SelectedToDoList.IconPath = _newToDoList.IconPath;

            // change parent name for all sub-tdl's of edited tdl
            foreach (ToDoList tdl in Context.SelectedToDoList.ToDoLists)
            {
                tdl.ParentName = Context.SelectedToDoList.Name;
            }

            Context.Database.UpdateToDoList(Context.SelectedToDoList);
            SerializationActions.Serialize(Context.Database, Context.Database.Name + ".xml");

            _ = MessageBox.Show("The to do list has been edited!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // for event handling
        public void IconListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IconPath = (sender as ListBox).SelectedItem as string;
        }
    }
}
