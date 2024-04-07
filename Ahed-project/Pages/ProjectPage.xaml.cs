using Ahed_project.Services.Global;
using Ahed_project.ViewModel.Pages;
using System;
using System.Windows.Controls;

namespace Ahed_project.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProjectPage.xaml
    /// </summary>
    public partial class ProjectPage : Page
    {
        public ProjectPage(ProjectPageViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void TextBox_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            GlobalFunctionsAndCallersService.ReRender(Convert.ToInt32(((TextBox)sender).Text));
        }

        private void MenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
