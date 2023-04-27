using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ToDoApp.Commands;
using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp.ViewModels
{
    public class TaskViewModel : ViewModelBase
    {
        public ViewModelContext Context { get; set; }
        public bool CanExecuteAction { get; set; } = false;

        private readonly Task _newTask = new Task();

        public string Name
        {
            get => _newTask.Name;
            set
            {
                _newTask.Name = value;
                CanExecuteAction = _newTask.Name != "";
                OnPropertyChanged(nameof(Name));
            }
        }

        public List<EPriority> Priorities { get; } = Enum.GetValues(typeof(EPriority)).Cast<EPriority>().ToList();
        public List<EStatus> Statuses { get; } = Enum.GetValues(typeof(EStatus)).Cast<EStatus>().ToList();

        private int _selectedPriorityIndex;
        public int SelectedPriorityIndex
        {
            get => _selectedPriorityIndex;
            set
            {
                _selectedPriorityIndex = value;
                _newTask.Priority = Priorities[_selectedPriorityIndex];
                OnPropertyChanged(nameof(SelectedPriorityIndex));
            }
        }


        private int _selectedStatusIndex;
        public int SelectedStatusIndex
        {
            get => _selectedStatusIndex;
            set
            {
                _selectedStatusIndex = value;
                _newTask.Status = Statuses[_selectedStatusIndex];
                OnPropertyChanged(nameof(SelectedStatusIndex));
            }
        }

        public Category Category
        {
            get => _newTask.Category;
            set
            {
                _newTask.Category = value;
                OnPropertyChanged(nameof(Category));
            }
        }
        public DateTime Deadline
        {
            get => _newTask.Deadline;
            set
            {
                _newTask.Deadline = value;
                OnPropertyChanged(nameof(Deadline));
            }
        }

        public string Description
        {
            get => _newTask.Description;
            set
            {
                _newTask.Description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public bool IsCompleted
        {
            get => _newTask.IsCompleted;
            set
            {
                _newTask.IsCompleted = value;
                OnPropertyChanged(nameof(IsCompleted));
            }
        }

        public ICommand ActionButtonCommand { get; }

        public TaskViewModel(ViewModelContext context)
        {
            Context = context;
            _newTask = new Task();
            if (Context.ActionType == EActionType.Add)
            {
                ActionButtonCommand = new RelayCommand(AddTask, parameter => CanExecuteAction);
            }
            else if (Context.ActionType == EActionType.Edit)
            {
                ActionButtonCommand = new RelayCommand(EditTask, parameter => CanExecuteAction);

                Name = Context.SelectedTask.Name;
                SelectedPriorityIndex = Priorities.IndexOf(Context.SelectedTask.Priority);
                SelectedStatusIndex = Statuses.IndexOf(Context.SelectedTask.Status);
                Category = Context.SelectedTask.Category;
                Deadline = Context.SelectedTask.Deadline;
                Description = Context.SelectedTask.Description;
                IsCompleted = Context.SelectedTask.IsCompleted;
            }
        }

        // command actions
        private void AddTask()
        {
            if (_newTask.IsDueToday())
            {
                Context.DueToday++;
            }
            if (_newTask.IsDueTomorrow())
            {
                Context.DueTomorrow++;
            }
            if (_newTask.IsOverdue())
            {
                Context.Overdue++;
            }
            if (_newTask.IsCompleted)
            {
                Context.Done++;
            }
            else
            {
                Context.ToBeDone++;
            }
            Context.SelectedToDoList.Tasks.Add(_newTask);
            Context.SaveDatabase();
            _ = MessageBox.Show("The task has been added!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void EditTask()
        {
            if (Context.SelectedTask.IsDueToday())
            {
                Context.DueToday--;
            }
            if (Context.SelectedTask.IsDueTomorrow())
            {
                Context.DueTomorrow--;
            }
            if (Context.SelectedTask.IsOverdue())
            {
                Context.Overdue--;
            }
            if (Context.SelectedTask.IsCompleted)
            {
                Context.Done--;
            }
            else
            {
                Context.ToBeDone--;
            }

            Context.SelectedTask.Name = Name;
            Context.SelectedTask.Priority = _newTask.Priority;
            Context.SelectedTask.IsCompleted = IsCompleted;
            Context.SelectedTask.Status = _newTask.Status;
            Context.SelectedTask.Category = Category;
            Context.SelectedTask.Deadline = Deadline;
            Context.SelectedTask.Description = Description;

            if (Context.SelectedTask.IsDueToday())
            {
                Context.DueToday++;
            }
            if (Context.SelectedTask.IsDueTomorrow())
            {
                Context.DueTomorrow++;
            }
            if (Context.SelectedTask.IsOverdue())
            {
                Context.Overdue++;
            }
            if (Context.SelectedTask.IsCompleted)
            {
                Context.Done++;
            }
            else
            {
                Context.ToBeDone++;
            }

            Context.SaveDatabase();

            _ = MessageBox.Show("The task has been edited!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
