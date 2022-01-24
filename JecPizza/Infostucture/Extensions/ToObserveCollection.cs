using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace JecPizza.Infostucture.Extensions
{
    public static class ToObserveCollection
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> src) => new ObservableCollection<T>(src);

        public static void AddFirst<T>(this ObservableCollection<T> src, T element) => src.Insert(0, element);
    }
}