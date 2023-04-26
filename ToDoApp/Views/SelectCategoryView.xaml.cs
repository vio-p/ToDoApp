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
    /// Interaction logic for SelectCategoryView.xaml
    /// </summary>
    public partial class SelectCategoryView : Window
    {
        public SelectCategoryView(ViewModelContext context)
        {
            DataContext = new SelectCategoryViewModel(context);
            InitializeComponent();
        }

        private void CategoriesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as SelectCategoryViewModel).CategoriesDataGrid_SelectionChanged(sender, e);
        }
    }
}
