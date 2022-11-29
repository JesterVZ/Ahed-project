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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ahed_project.UserControlsCustom
{
    /// <summary>
    /// Логика взаимодействия для CustonCheck.xaml
    /// </summary>
    public partial class CustonCheck : System.Windows.Controls.UserControl
    {
        public CustonCheck()
        {
            InitializeComponent();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CustomTextBox.IsEnabled = false;
            CustomTextBox.Background = new SolidColorBrush(Color.FromRgb(249, 239, 229));
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CustomTextBox.IsEnabled = true;
            CustomTextBox.Background = new SolidColorBrush(Colors.White);
        }
    }
}
