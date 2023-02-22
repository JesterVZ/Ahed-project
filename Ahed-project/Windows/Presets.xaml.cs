using Ahed_project.ViewModel.Windows;
using System.Windows;
using System.Windows.Input;

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
            PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };
        }
    }
}
