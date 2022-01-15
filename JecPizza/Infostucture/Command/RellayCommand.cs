using System;
using JecPizza.Infostucture.Command.Base;

namespace JecPizza.Infostucture.Command
{
    public class RellayCommand : BaseCommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RellayCommand(Action<object> _execute, Predicate<object> _canExecute)
        {
            this._execute = _execute ?? throw new ArgumentNullException(nameof(_execute));
            this._canExecute = _canExecute;

        }

        public RellayCommand(Action<object> _execute) : this(_execute, null) { }







        public override bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

        public override void Execute(object parameter) => _execute(parameter);
    }
}