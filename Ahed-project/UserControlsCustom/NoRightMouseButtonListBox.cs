using Ahed_project.MasterData.CalculateClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ahed_project.UserControl
{
    internal class NoRightMouseButtonListBox:ListBox
    {
        public static readonly DependencyProperty dependencyProperty = DependencyProperty.Register("RightButtonClicked", typeof(CalculationFull), typeof(NoRightMouseButtonListBox), new PropertyMetadata(null, new PropertyChangedCallback(ChangeRightButtonClickedProperty)));
        public CalculationFull RightButtonClicked
        {
            get => (CalculationFull)GetValue(dependencyProperty);
            set => SetValue(dependencyProperty, value);
        }

        private static void ChangeRightButtonClickedProperty(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as NoRightMouseButtonListBox).RightButtonClicked = (CalculationFull)e.NewValue;
        }

        protected override void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e)
        {
            var originalSource = (Border)e.OriginalSource;
            RightButtonClicked = originalSource.DataContext as CalculationFull;
            e.Handled = true;
        }
    }
}
