using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Ahed_project.UserControlsCustom
{
    public class CustomTextBox : TextBox
    {
        public static readonly DependencyProperty dependencyProperty = DependencyProperty.Register("IsError", typeof(bool), typeof(TextBox), new PropertyMetadata(false, new PropertyChangedCallback(ChangeErrorProperty)));

        public bool IsError
        {
            get => (bool)GetValue(dependencyProperty);
            set => SetValue(dependencyProperty, value);
        }

        private static void ChangeErrorProperty(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            (d as CustomTextBox).IsError = (bool)e.NewValue;
        }
    }
}
