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
    /// Interaction logic for FindTaskView.xaml
    /// </summary>
    public partial class FindTaskView : Window
    {
        public FindTaskView(ViewModelContext context)
        {
            DataContext = new FindTaskViewModel(context);
            InitializeComponent();
        }
    }
}
