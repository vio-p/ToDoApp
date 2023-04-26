using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ToDoApp.Commands;
using ToDoApp.Models;

namespace ToDoApp.ViewModels
{
    public class FindTaskViewModel : ViewModelBase
    {
        public ViewModelContext Context { get; set; }

        public bool CanExecuteAction { get; set; }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                if (FindByName)
                {
                    CanExecuteAction = _name != null && _name != "";
                }
                OnPropertyChanged(nameof(Name));
            }
        }

        private DateTime _deadline;
        public DateTime Deadline
        {
            get => _deadline;
            set
            {
                _deadline = value;
                OnPropertyChanged(nameof(Deadline));
            }
        }

        private bool _findByName = true;
        public bool FindByName
        {
            get => _findByName;
            set
            {
                _findByName = value;
                OnPropertyChanged(nameof(FindByName));
            }
        }

        private bool _findByDeadline;
        public bool FindByDeadline
        {
            get => _findByDeadline;
            set
            {
                _findByDeadline = value;
                CanExecuteAction = true;
                OnPropertyChanged(nameof(FindByDeadline));
            }
        }

        private ObservableCollection<Task> _foundTasks;
        public ObservableCollection<Task> FoundTasks
        {
            get => _foundTasks;
            set
            {
                _foundTasks = value;
                OnPropertyChanged(nameof(FoundTasks));
            }
        }

        public ICommand FindCommand { get; }

        public FindTaskViewModel(ViewModelContext context)
        {
            Context = context;
            Deadline = DateTime.Now;
            FoundTasks = new ObservableCollection<Task>();

            FindCommand = new RelayCommand(FindTasks, parameter => CanExecuteAction);
        }

        private void FindTasks()
        {
            if (FindByName)
            {
                FoundTasks = new ObservableCollection<Task>(Context.Database.GetAllTasks().Where(task => task.Name.Contains(Name.Trim())));
            }
            else if (FindByDeadline)
            {
                FoundTasks = new ObservableCollection<Task>(Context.Database.GetAllTasks().Where(task => task.Deadline.Date == Deadline.Date));
            }
        }
    }
}
