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
        public static readonly DependencyProperty dependencyPropertyText = DependencyProperty.Register("InputText", typeof(string), typeof(CustonCheck), new PropertyMetadata("", new PropertyChangedCallback(ChangeTextProperty)));
        public static readonly DependencyProperty dependencyPropertyIsChecked = DependencyProperty.Register("IsChecked", typeof(int), typeof(CustonCheck), new PropertyMetadata(0, new PropertyChangedCallback(ChangeIsCheckedProperty)));

        public int IsChecked
        {
            get => (int)GetValue(dependencyPropertyIsChecked);
            set
            {
                SetValue(dependencyPropertyIsChecked, value);
                switch (value)
                {
                    case 0:
                        CheckBox.IsChecked = false;
                        CustomTextBox.IsEnabled = false;
                        CustomTextBox.Background = new SolidColorBrush(Color.FromRgb(249, 239, 229));
                        break;
                    case 1:
                        CheckBox.IsChecked = true;
                        CustomTextBox.IsEnabled = true;
                        CustomTextBox.Background = new SolidColorBrush(Colors.White);
                        break;

                }
            }
        }

        public string InputText
        {
            get => (string)GetValue(dependencyPropertyText);
            set { 
                SetValue(dependencyPropertyText, value);
                CustomTextBox.Text = value;
            }
        }

        public CustonCheck()
        {
            InitializeComponent();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CustomTextBox.IsEnabled = true;
            IsChecked = 1;
            CustomTextBox.Background = new SolidColorBrush(Colors.White);
            //CustomTextBox.Background = new SolidColorBrush(Color.FromRgb(249, 239, 229));
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CustomTextBox.IsEnabled = false;
            IsChecked = 0;
            CustomTextBox.Background = new SolidColorBrush(Color.FromRgb(249, 239, 229));
        }
        private static void ChangeTextProperty(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as CustonCheck).InputText = (string)e.NewValue;
        }
        private static void ChangeIsCheckedProperty(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as CustonCheck).IsChecked = (int)e.NewValue;
        }

        private void CustomTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            InputText = CustomTextBox.Text;
        }
    }
}
