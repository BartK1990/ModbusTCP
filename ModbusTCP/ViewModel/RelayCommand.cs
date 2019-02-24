using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusTCP.ViewModel
{
    using System.Windows.Input;

    public class RelayCommand : ICommand
    {
        private Action _action;
        private Func<bool> canExecute;
        public RelayCommand(Action action, Func<bool> canExecute)
        {
            this._action = action;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            return canExecute();
        }

        public void Execute(object parameter)
        {
            _action();
        }
    }
}
