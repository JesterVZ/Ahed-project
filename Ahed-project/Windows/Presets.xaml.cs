using Ahed_project.ViewModel.Windows;
using System.Windows;

namespace Ahed_project.Windows
{
    /// <summary>
    /// Логика взаимодействия для Presets.xaml
    /// </summary>
    public partial class Presets : Window
    {
        public Presets(PresetsWindowViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
