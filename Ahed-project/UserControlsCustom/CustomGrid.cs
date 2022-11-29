using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Ahed_project.UserControl
{
    public class CustomGrid : Grid
    {
        public static readonly DependencyProperty dependencyProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(CustomGrid), new PropertyMetadata(false, new PropertyChangedCallback(ChangeIsOpenProperty)));

        public bool IsOpen
        {
            get => (bool)GetValue(dependencyProperty);
            set => SetValue(dependencyProperty, value);
        }

        private static void ChangeIsOpenProperty(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as CustomGrid).IsOpen = (bool)e.NewValue;
        }
    }
}
