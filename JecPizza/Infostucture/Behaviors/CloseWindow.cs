using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace JecPizza.Infostucture.Behaviors
{
    public class CloseWindow : Behavior<Button>
    {
        protected override void OnAttached() => AssociatedObject.Click += OnCloseBtnClick;

        protected override void OnDetaching() => AssociatedObject.Click -= OnCloseBtnClick;

        private void OnCloseBtnClick(object Sender, RoutedEventArgs E)
        {
            var btn = AssociatedObject;
            var window = btn.FindVisualRoot() as Window;
            window?.Close();
        }
    }
}
