using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace AS_Client.ViewModels
{
    public class RelayCommand<T> : ICommand where T : class
    {
        private readonly Predicate<T> _canExecute;
        private readonly Action<T> _execute;

        public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            _canExecute = canExecute;
            _execute = execute;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke(parameter as T) ?? true;
        }

        public void Execute(object parameter)
        {
            _execute(parameter as T);
        }
    }

    public class RelayCommand : RelayCommand<object>
    {
        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null) : base(execute, canExecute)
        {
            
        }
    }
}
