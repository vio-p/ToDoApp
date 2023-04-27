using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ToDoApp.ViewModels;

namespace ToDoApp.Views
{
    /// <summary>
    /// Interaction logic for OpenDatabaseView.xaml
    /// </summary>
    public partial class OpenDatabaseView : Window
    {
        public OpenDatabaseView(ViewModelContext context)
        {
            DataContext = new OpenDatabaseViewModel(context);
            InitializeComponent();
        }

        private void DatabasesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as OpenDatabaseViewModel).DatabasesDataGrid_SelectionChanged(sender, e);
        }
    }
}
