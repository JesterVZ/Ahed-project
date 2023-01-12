using Ahed_project.ViewModel.Windows;
using System.Windows;

namespace Ahed_project
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
            vm.CloseWindow = ()=>
            {
                this.Close();
            };
        }
    }
}
