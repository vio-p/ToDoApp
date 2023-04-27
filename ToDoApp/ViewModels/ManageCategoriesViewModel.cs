using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ToDoApp.Commands;
using ToDoApp.Models;

namespace ToDoApp.ViewModels
{
    public class ManageCategoriesViewModel : ViewModelBase
    {
        public ViewModelContext Context { get; set; }
        public ObservableCollection<Category> Categories { get; set; }
        public ICommand SaveChangesCommand { get; }

        public ManageCategoriesViewModel(ViewModelContext context)
        {
            Context = context;
            Categories = Context.Database.Categories;
            SaveChangesCommand = new RelayCommand(SaveChanges);
        }

        // command actions
        private void SaveChanges()
        {
            if (Categories.Last().Id == 0)
            {
                for (int index = 0; index < Categories.Count; index++)
                {
                    if (Categories[index].Id == 0)
                    {
                        if (index == 0)
                        {
                            Categories[index].Id = 1;
                        }
                        else
                        {
                            Categories[index].Id = Categories[index - 1].Id + 1;
                        }
                    }
                }
            }
            Context.SaveDatabase();
            _ = MessageBox.Show("Your changes have been saved!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // for event handling
        public void CategoriesDataGrid_PreviewDeleteCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == DataGrid.DeleteCommand)
            {
                if (!(MessageBox.Show("Are you sure you want to delete the category?", "Please confirm.", MessageBoxButton.YesNo) == MessageBoxResult.Yes))
                {
                    // cancel delete
                    e.Handled = true;
                    return;
                }
                Category deletedCategory = (sender as DataGrid).SelectedItem as Category;
                List<Task> allTasks = Context.Database.GetAllTasks();
                foreach (Task task in allTasks)
                {
                    if (task.Category == deletedCategory)
                    {
                        task.Category = null;
                        task.CategoryId = 0;
                    }
                }
            }
        }

        public void CategoriesDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                TextBox cellTextBox = (TextBox)e.EditingElement;
                string text = cellTextBox.Text;
                if (text == "")
                {
                    _ = MessageBox.Show("Invalid category!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    e.Cancel = true;
                    return;
                }
                if (Categories.Where(category => category.Name == text).ToList().Count == 1)
                {
                    _ = MessageBox.Show("Category already exists!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    e.Cancel = true;
                }
            }
        }
    }
}
