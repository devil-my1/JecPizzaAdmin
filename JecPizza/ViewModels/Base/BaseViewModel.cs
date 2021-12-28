using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace JecPizza.ViewModels.Base
{
    internal abstract class BaseViewModel : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _Disposed;

        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string ProperyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(ProperyName);
            return true;
        }

        ~BaseViewModel() => Dispose(true);

        public void Dispose()
        {
            Dispose(true); 
        }

        protected virtual void Dispose(bool Disposing)
        {
            if(!Disposing || _Disposed) return;
            _Disposed = true;
        }
    }
}