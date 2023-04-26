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
    /// Interaction logic for ManageCategoriesView.xaml
    /// </summary>
    public partial class ManageCategoriesView : Window
    {
        public ManageCategoriesView(ViewModelContext context)
        {
            DataContext = new ManageCategoriesViewModel(context);
            InitializeComponent();
        }

        private void CategoriesDataGrid_PreviewDeleteCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            (DataContext as ManageCategoriesViewModel).CategoriesDataGrid_PreviewDeleteCommandHandler(sender, e);
        }

        private void CategoriesDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            (DataContext as ManageCategoriesViewModel).CategoriesDataGrid_CellEditEnding(sender, e);
        }
    }
}
