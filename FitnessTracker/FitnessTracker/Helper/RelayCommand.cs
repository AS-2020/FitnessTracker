using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace FitnessTracker.Helper
{
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action<object> _execute;

        public RelayCommand(Action<object> execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _execute?.Invoke(parameter);
        }
    }
}
