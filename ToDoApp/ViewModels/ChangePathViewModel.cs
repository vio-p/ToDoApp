using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ToDoApp.Commands;
using ToDoApp.Models;

namespace ToDoApp.ViewModels
{
    public class ChangePathViewModel : ViewModelBase
    {
        public ViewModelContext Context { get; set; }
        public ICommand ChangePathCommand { get; }

        public ToDoList SelectedParent { get; set; }

        public ChangePathViewModel(ViewModelContext context)
        {
            Context = context;
            ChangePathCommand = new RelayCommand(ChangePath, parameter => SelectedParent != null && !SelectedParent.IsOrIsChildOf(Context.SelectedToDoList));
        }

        public void ChangePath()
        {
            Context.SelectedToDoList.ParentName = SelectedParent.Name;
            ToDoList selectedToDoList = Context.SelectedToDoList;
            Context.Database.DeleteToDoList(selectedToDoList);
            SelectedParent.ToDoLists.Add(selectedToDoList);
            Context.SaveDatabase();
            Context.OpenWindow.Close();
        }


        // for event handling
        public void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedParent = e.NewValue as ToDoList;
        }
    }
}
