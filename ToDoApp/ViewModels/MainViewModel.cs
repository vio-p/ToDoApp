using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDoApp.Commands;
using ToDoApp.Views;

namespace ToDoApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ICommand ManageCategoriesCommand { get; }

        public MainViewModel()
        {
            ManageCategoriesCommand = new ActionCommand(ShowManageCategoriesView);
        }

        // command actions
        private void ShowManageCategoriesView()
        {
            ManageCategoriesView manageCategoriesView = new ManageCategoriesView();
            manageCategoriesView.Show();
        }
    }
}
