using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Ahed_project.UserControlsCustom
{
    /// <summary>
    /// Логика взаимодействия для CustomCheck.xaml
    /// </summary>
    public partial class CustomCheck : System.Windows.Controls.UserControl
    {
        public static readonly DependencyProperty dependencyPropertyText = DependencyProperty.Register("InputText", typeof(string), typeof(CustomCheck), new PropertyMetadata("", new PropertyChangedCallback(ChangeTextProperty)));
        public static readonly DependencyProperty dependencyPropertyIsChecked = DependencyProperty.Register("IsChecked", typeof(int), typeof(CustomCheck), new PropertyMetadata(0, new PropertyChangedCallback(ChangeIsCheckedProperty)));

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
                        CustomTextBox.IsReadOnly = true;
                        CustomTextBox.Background = new SolidColorBrush(Color.FromRgb(249, 239, 229));
                        break;
                    case 1:
                        CheckBox.IsChecked = true;
                        CustomTextBox.IsReadOnly = false;
                        CustomTextBox.Background = new SolidColorBrush(Colors.White);
                        break;

                }
            }
        }

        public string InputText
        {
            get => (string)GetValue(dependencyPropertyText);
            set
            {
                SetValue(dependencyPropertyText, value);
                CustomTextBox.Text = value;
            }
        }

        public CustomCheck()
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
            (d as CustomCheck).InputText = (string)e.NewValue;
        }
        private static void ChangeIsCheckedProperty(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as CustomCheck).IsChecked = (int)e.NewValue;
        }

        private void CustomTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            InputText = CustomTextBox.Text;
        }
    }
}
