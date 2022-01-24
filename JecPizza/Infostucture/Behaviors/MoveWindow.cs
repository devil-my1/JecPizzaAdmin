using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace JecPizza.Infostucture.Behaviors
{
    public class MoveWindow : Behavior<Grid>
    {
        protected override void OnAttached() => AssociatedObject.MouseLeftButtonDown += OnWindowMove;


        protected override void OnDetaching() => AssociatedObject.MouseLeftButtonDown -= OnWindowMove;


        private void OnWindowMove(object Sender, MouseButtonEventArgs E)
        {
            switch (E.ClickCount)
            {
                case 1:
                    OnDragMove();
                    break;
                default:
                    OnMaximaze();
                    break;
            }
        }

        private void OnDragMove() => (AssociatedObject.FindVisualRoot() as Window)?.DragMove();


        private void OnMaximaze()
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
