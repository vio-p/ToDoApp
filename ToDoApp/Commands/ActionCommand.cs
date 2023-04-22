using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp.Commands
{
    public class ActionCommand : CommandBase
    {
        private readonly Action _commandAction;
        public override void Execute(object parameter)
        {
            _commandAction();
        }

        public ActionCommand(Action commandAction)
        {
            _commandAction = commandAction;
        }
    }
}
