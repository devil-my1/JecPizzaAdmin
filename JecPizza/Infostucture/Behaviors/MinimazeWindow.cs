using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace JecPizza.Infostucture.Behaviors
{
    public class MinimazeWindow : Behavior<Button>
    {
        protected override void OnAttached() => AssociatedObject.Click += OnMinimaze;

        protected override void OnDetaching() => AssociatedObject.Click -= OnMinimaze;


        private void OnMinimaze(object Sender, RoutedEventArgs E)
        {
            var element = AssociatedObject.FindVisualRoot() as Window;

            switch (element?.WindowState)
            {
                case WindowState.Normal:
                    element.WindowState = WindowState.Minimized;
                    break;
                case WindowState.Minimized:
                    element.WindowState = WindowState.Normal;
                    break;
            }
        }
    }
}
