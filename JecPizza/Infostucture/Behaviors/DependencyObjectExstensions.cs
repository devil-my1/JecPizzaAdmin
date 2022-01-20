using System.Windows;
using System.Windows.Media;

namespace JecPizza.Infostucture.Behaviors
{
    public static class DependencyObjectExstensions
    {
        public static DependencyObject FindVisualRoot(this DependencyObject obj)
        {
            do
            {
                var element = VisualTreeHelper.GetParent(obj);
                if (element is null) return obj;
                obj = element;

            }
            while (true);
        }

        public static DependencyObject FindLogicalRoot(this DependencyObject obj)
        {
            do
            {
                var element = LogicalTreeHelper.GetParent(obj);
                if (element is null) return obj;
                obj = element;

            }
            while (true);
        }

        public static T FindVisualParent<T>(this DependencyObject obj) where T : DependencyObject
        {
            do
            {
                var parent = VisualTreeHelper.GetParent(obj);
                if (parent is T) return parent as T;
            }
            while (true);
        }
    }
}
