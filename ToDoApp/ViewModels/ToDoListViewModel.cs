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
        public ViewModelContext Context { get; set; }
        public List<string> IconPaths { get; set; }
        public bool CanExecuteAction { get; set; } = false;

        private int _selectedIconIndex;
        public int SelectedIconIndex
        {
            get => _selectedIconIndex;
            set
            {
                _selectedIconIndex = value;
                _newToDoList.IconPath = IconPaths[SelectedIconIndex];
                OnPropertyChanged(nameof(SelectedIconIndex));
            }
        }
        

        private ToDoList _newToDoList = new ToDoList();
        public string Name
        {
            get => _newToDoList.Name;
            set
            {
                _newToDoList.Name = value;
                CanExecuteAction = _newToDoList.Name != "" && _newToDoList.Name != null;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string ParentName
        {
            get => _newToDoList.ParentName;
            set => _newToDoList.ParentName = value;
        }

        public ICommand ActionButtonCommand { get; }

        public ToDoListViewModel(ViewModelContext context)
        {
            Context = context;
            IconPaths = new List<string>(Directory.GetFiles(Environment.CurrentDirectory + @"..\..\..\Resources\ToDoListIcons", "*.png", SearchOption.TopDirectoryOnly));

            if (Context.ActionType == EActionType.Add)
            {
                SelectedIconIndex = 0;
                if (Context.SelectedToDoList == null) // adding root to do list
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
            else if (Context.ActionType == EActionType.Edit)
            {
                ActionButtonCommand = new RelayCommand(EditToDoList, parameter => CanExecuteAction);

                Name = Context.SelectedToDoList.Name;
                SelectedIconIndex = IconPaths.IndexOf(Context.SelectedToDoList.IconPath);
                ParentName = Context.SelectedToDoList.ParentName;
            }
        }

        // command actions
        private void AddRootToDoList()
        {
            Context.Database.RootToDoLists.Add(_newToDoList);
            Context.SaveDatabase();
            _ = MessageBox.Show("The to do list has been added!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            _newToDoList = new ToDoList();
            SelectedIconIndex = 0;
        }

        private void AddSubToDoList()
        {
            Context.SelectedToDoList.ToDoLists.Add(_newToDoList);
            Context.SaveDatabase();
            _ = MessageBox.Show("The to do list has been added!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            _newToDoList = new ToDoList();
            SelectedIconIndex = 0;
        }

        private void EditToDoList()
        {
            Context.SelectedToDoList.Name = Name;
            Context.SelectedToDoList.IconPath = _newToDoList.IconPath;

            // change parent name for all sub-tdl's of edited tdl
            foreach (ToDoList tdl in Context.SelectedToDoList.ToDoLists)
            {
                tdl.ParentName = Context.SelectedToDoList.Name;
            }

            Context.SaveDatabase();

            _ = MessageBox.Show("The to do list has been edited!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
