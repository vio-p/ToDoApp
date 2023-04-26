using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ToDoApp.Commands;
using ToDoApp.Models;

namespace ToDoApp.ViewModels
{
    public class SelectCategoryViewModel : ViewModelBase
    {
        public ViewModelContext Context { get; set; }
        public ObservableCollection<Category> Categories { get; set; }
        public ICommand SelectCategoryCommand { get; }

        public SelectCategoryViewModel(ViewModelContext context)
        {
            Context = context;
            Categories = Context.Database.Categories;
            SelectCategoryCommand = new RelayCommand(SelectCategory);
        }

        private void SelectCategory()
        {
            if (Context.CategoryToFilterBy == null)
            {
                _ = MessageBox.Show("No category selected!", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            Context.OpenWindow.Close();
            Context.DisplayedTasks = new ObservableCollection<Task>(Context.SelectedToDoList.Tasks.Where(task => task.Category == Context.CategoryToFilterBy));
        }

        // for event handling
        public void CategoriesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Context.CategoryToFilterBy = (sender as DataGrid).SelectedItem as Category;
        }
    }
}
