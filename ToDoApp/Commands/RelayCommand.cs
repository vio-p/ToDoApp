using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp.Commands
{
    public class RelayCommand : CommandBase
    {
        private readonly Action _commandTask;
        private readonly Predicate<object> _canExecuteTask;

        public RelayCommand(Action commandTask, Predicate<object> canExecuteTask)
        {
            _commandTask = commandTask;
            _canExecuteTask = canExecuteTask;
        }

        private static bool DefaultCanExecute(object parameter)
        {
            return true;
        }

        public RelayCommand(Action commandTask)
            : this(commandTask, DefaultCanExecute)
        {
            _commandTask = commandTask;
        }

        public override bool CanExecute(object parameter)
        {
            return _canExecuteTask != null && _canExecuteTask(parameter);
        }

        public override void Execute(object parameter)
        {
            _commandTask();
        }
    }
}
