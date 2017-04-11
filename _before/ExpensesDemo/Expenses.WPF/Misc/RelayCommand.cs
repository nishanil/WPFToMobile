using System;
using System.Windows.Input;

namespace Expenses.WPF
{

    public class RelayCommand : ICommand
    {
        private readonly Action _action;
        private readonly Func<bool> _canExecute;

        public event EventHandler CanExecuteChanged;

        private RelayCommand() { }

        public RelayCommand(Action action)
        {
            this._action = action;
            this._canExecute = () => { return true; };
        }

        public RelayCommand(Action action, Func<bool> canExecute)
        {
            this._action = action;
            this._canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this._canExecute();
        }

        public void Execute(object parameter)
        {
            this._action();
        }

        public void OnCanExecuteChanged()
        {
            var evt = CanExecuteChanged;
            if (evt != null)
            {
                evt.Invoke(this, null);
            }
        }
    }

    public class RelayCommand<T> : ICommand where T : class
    {
        private readonly Action<T> _action;
        private readonly Func<T, bool> _canExecute;

        public event EventHandler CanExecuteChanged;

        private RelayCommand() { }

        public RelayCommand(Action<T> action)
        {
            this._action = action;
            this._canExecute = (p) => { return true; };
        }

        public RelayCommand(Action<T> action, Func<T, bool> canExecute)
        {
            this._action = action;
            this._canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this._canExecute(parameter as T);
        }

        public void Execute(object parameter)
        {
            this._action(parameter as T);
        }

        public void OnCanExecuteChanged()
        {
            var evt = CanExecuteChanged;
            if (evt != null)
            {
                evt.Invoke(this, null);
            }
        }
    }
}
