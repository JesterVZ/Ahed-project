using Ahed_project.ViewModel.Windows;
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

namespace Ahed_project.Windows
{
    /// <summary>
    /// Логика взаимодействия для GeometryWindow.xaml
    /// </summary>
    public partial class GeometryWindow : Window
    {
        private GeometryWindowViewModel _geometryWindowViewModel;
        public GeometryWindow(GeometryWindowViewModel vm)
        {
            InitializeComponent();
            _geometryWindowViewModel = vm;
            DataContext = _geometryWindowViewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _geometryWindowViewModel.TextBox = "";
        }
    }
}
