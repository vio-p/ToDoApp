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
    public class ManageCategoriesViewModel : ViewModelBase
    {
        public ObservableCollection<Category> Categories { get; set; }
        public ICommand SaveChangesCommand { get; }

        private readonly string _categoriesFilePath;
        private int _lastCategoryId;

        public ManageCategoriesViewModel()
        {
            _categoriesFilePath = "categories.xml";
            if (!File.Exists(_categoriesFilePath))
            {
                Categories = new ObservableCollection<Category>();
                SerializationActions.Serialize(Categories, _categoriesFilePath);
            }
            Categories = SerializationActions.Deserialize<ObservableCollection<Category>>("categories.xml");
            _lastCategoryId = Categories.Last().Id;
            SaveChangesCommand = new ActionCommand(SaveChanges);
        }

        // command actions
        private void SaveChanges()
        {
            bool passedFirstNewCategory = false;
            if (Categories.Last().Id == 0)
            {
                for (int index = 0; index < Categories.Count; index++)
                {
                    if (Categories[index].Id == 0)
                    {
                        if (!passedFirstNewCategory)
                        {
                            Categories[index].Id = _lastCategoryId + 1;
                            passedFirstNewCategory = true;
                        }
                        else
                        {
                            Categories[index].Id = Categories[index - 1].Id + 1;
                        }
                    }
                }
            }
            if (passedFirstNewCategory)
            {
                _lastCategoryId = Categories.Last().Id;
            }
            SerializationActions.Serialize(Categories, _categoriesFilePath);
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
                if (Categories.FirstOrDefault(category => category.Name == text) != null)
                {
                    _ = MessageBox.Show("Category already exists!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    e.Cancel = true;
                }
            }
        }
    }
}
