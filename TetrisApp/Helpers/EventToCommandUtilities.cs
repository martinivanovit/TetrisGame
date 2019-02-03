using System.Windows;
using System.Windows.Input;

namespace Tetris
{
    //To improve: Use WeakEventListener to avoid memory leaks. Introduce command parameter properties.
    public static class EventToCommandUtilities
    { 
        public static readonly DependencyProperty MouseLeftButtonDownCommandProperty =
            DependencyProperty.RegisterAttached(
                "MouseLeftButtonDownCommand", 
                typeof(ICommand), 
                typeof(EventToCommandUtilities), 
                new PropertyMetadata(null, OnMouseLeftButtonDownCommandChanged));

        public static ICommand GetMouseLeftButtonDownCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(MouseLeftButtonDownCommandProperty);
        }

        public static void SetMouseLeftButtonDownCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(MouseLeftButtonDownCommandProperty, value);
        }

        private static void OnMouseLeftButtonDownCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (FrameworkElement)d;
            if (e.NewValue != null)
            {
                element.MouseLeftButtonDown += Element_MouseLeftButtonDown;
            }
            else
            {
                element.MouseLeftButtonDown -= Element_MouseLeftButtonDown;
            }
        }

        private static void Element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var command = GetMouseLeftButtonDownCommand(element);
            if (command != null)
            {
                command.Execute(null);
            }
        }
    }
}
