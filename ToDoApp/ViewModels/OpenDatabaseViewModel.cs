using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using ToDoApp.Commands;
using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp.ViewModels
{
    public class OpenDatabaseViewModel : ViewModelBase
    {
        public ViewModelContext Context { get; set; }
        public ICommand OpenCommand { get; }

        public Database SelectedDatabase { get; set; }
        public ObservableCollection<Database> Databases { get; set; }

        private readonly List<string> _databasePaths = new List<string>(Directory.GetFiles("Databases", "*.xml", SearchOption.TopDirectoryOnly));


        public OpenDatabaseViewModel(ViewModelContext context)
        {
            Context = context;
            Databases = new ObservableCollection<Database>();
            foreach (string path in _databasePaths)
            {
                Databases.Add(SerializationService.Deserialize<Database>(path));
            }

            OpenCommand = new RelayCommand(OpenDatabase, parameter => SelectedDatabase != null);
        }

        public void OpenDatabase()
        {
            Context.Database = SelectedDatabase;
            File.WriteAllText(@"Databases\last_opened.txt", SelectedDatabase.Path);
            Context.OpenWindow.Close();
        }

        // for event handling
        public void DatabasesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedDatabase = (sender as DataGrid).SelectedItem as Database;

        }
    }
}
