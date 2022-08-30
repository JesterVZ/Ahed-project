using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ahed_project.MasterData
{
    public class TextBoxBehavior
    {
        public static DependencyProperty OnLostFocusProperty = DependencyProperty.RegisterAttached(
           "OnLostFocus",
           typeof(ICommand),
           typeof(TextBoxBehavior),
           new UIPropertyMetadata(TextBoxBehavior.OnLostFocus));

        public static void SetOnLostFocus(DependencyObject target, ICommand value)
        {
            target.SetValue(TextBoxBehavior.OnLostFocusProperty, value);
        }

        /// <summary>
        /// PropertyChanged callback for OnDoubleClickProperty.  Hooks up an event handler with the 
        /// actual target.
        /// </summary>
        private static void OnLostFocus(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var element = target as TextBox;
            if (element == null)
            {
                throw new InvalidOperationException("This behavior can be attached to a TextBox item only.");
            }

            if ((e.NewValue != null) && (e.OldValue == null))
            {
                element.LostFocus += OnPreviewLostFocus;
            }
            else if ((e.NewValue == null) && (e.OldValue != null))
            {
                element.LostFocus -= OnPreviewLostFocus;
            }
        }

        private static void OnPreviewLostFocus(object sender, RoutedEventArgs e)
        {
            UIElement element = (UIElement)sender;
            ICommand command = (ICommand)element.GetValue(TextBoxBehavior.OnLostFocusProperty);
            if (command != null)
            {
                command.Execute(e);
            }
        }
    }
}
