using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace JecPizza.Infostucture.Behaviors
{
    public class MaximazeWindow : Behavior<Button>
    {
        protected override void OnAttached() => AssociatedObject.Click += OnMaximaze;


        protected override void OnDetaching() => AssociatedObject.Click -= OnMaximaze;

        private void OnMaximaze(object Sender, RoutedEventArgs E)
        {
            var element = AssociatedObject.FindVisualRoot() as Window;

            switch (element?.WindowState)
            {
                case WindowState.Normal:
                    element.WindowState = WindowState.Maximized;
                    break;
                case WindowState.Maximized:
                    element.WindowState = WindowState.Normal;
                    break;
            }
        }

    }
}
