using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Ahed_project.UserControl
{
    public class CustomColumnDefinition : ColumnDefinition
    {
        public static readonly DependencyProperty dependencyProperty = DependencyProperty.Register("IsColumnVisible", typeof(bool), typeof(CustomGrid), new PropertyMetadata(false, new PropertyChangedCallback(ChangeIsColumnVisible)));

        public bool IsColumnVisible
        {
            get => (bool)GetValue(dependencyProperty);
            set => SetValue(dependencyProperty, value);
        }

        private static void ChangeIsColumnVisible(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as CustomColumnDefinition).IsColumnVisible = (bool)e.NewValue;
        }
    }
}
