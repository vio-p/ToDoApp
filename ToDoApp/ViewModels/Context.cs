using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace ToDoApp.ViewModels
{
    public class Context : ViewModelBase
    {
        private Database _database;
        public Database Database
        {
            get => _database;
            set
            {
                _database = value;
                OnPropertyChanged(nameof(Database));
            }
        }
        public EActionType ActionType { get; set; }

        public bool _isSelectedToDoList = false;
        public bool IsSelectedToDoList
        {
            get => _isSelectedToDoList;
            set => _isSelectedToDoList = value;
        }

        private ToDoList _selectedToDoList;
        public ToDoList SelectedToDoList
        {
            get => _selectedToDoList;
            set
            {
                _selectedToDoList = value;
                _isSelectedToDoList = _selectedToDoList != null;
            }
        }

        public Context(Database database)
        {
            Database = database;
            ActionType = EActionType.None;
            SelectedToDoList = null;
        }
    }
}
