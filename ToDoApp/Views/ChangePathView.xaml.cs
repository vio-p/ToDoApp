﻿using System;
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
    /// Interaction logic for ChangePathView.xaml
    /// </summary>
    public partial class ChangePathView : Window
    {
        public ChangePathView(ViewModelContext context)
        {
            DataContext = new ChangePathViewModel(context);
            InitializeComponent();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            (DataContext as ChangePathViewModel).TreeView_SelectedItemChanged(sender, e);
        }
    }
}
