using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ToDoApp.Commands;
using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp.ViewModels
{
    public class CreateDatabaseViewModel : ViewModelBase
    {
        private string _name;

        private bool CanExecuteAction { get; set; } = false;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                CanExecuteAction = IsValidFileName(Name);
                OnPropertyChanged(nameof(Name));
            }
        }

        public ICommand CreateCommand { get; }

        private readonly List<string> _databasePaths = new List<string>(Directory.GetFiles("Databases", "*.xml", SearchOption.TopDirectoryOnly));

        public CreateDatabaseViewModel()
        {
            CreateCommand = new RelayCommand(CreateDatabase, parameter => CanExecuteAction);
        }

        private void CreateDatabase()
        {
            if (_databasePaths.Contains(@"Databases\" + Name + ".xml"))
            {
                _ = MessageBox.Show("There is already an unarchived database with this name!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Database newDatabase = new Database(Name);
            SerializationService.Serialize(newDatabase, newDatabase.Path);
            _ = MessageBox.Show("The database has been created!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static bool IsValidFileName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }
            if (name.Length > 1 && name[1] == ':')
            {
                if (name.Length < 4 || name.ToLower()[0] < 'a' || name.ToLower()[0] > 'z' || name[2] != '\\')
                {
                    return false;
                }
                name = name.Substring(3);
            }
            if (name.StartsWith("\\\\"))
            {
                name = name.Substring(1);
            }
            if (name.EndsWith("\\") || !name.Trim().Equals(name) || name.Contains("\\\\") ||
                name.IndexOfAny(Path.GetInvalidFileNameChars().Where(x => x != '\\').ToArray()) >= 0)
            {
                return false;
            }
            return true;
        }
    }
}
