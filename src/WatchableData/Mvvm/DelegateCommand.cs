using System;
using System.Windows.Input;

namespace WatchableData.Mvvm
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public DelegateCommand(Action execute, Func<bool> canExecute)
            : this(x => execute(), x => canExecute())
        {
        }

        public DelegateCommand(Action<object> execute)
            : this(execute, x => true)
        {
        }

        public DelegateCommand(Action execute)
            : this(x => execute())
        {
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
