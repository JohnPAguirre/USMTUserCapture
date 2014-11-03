using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace _335thUserCapture.ViewModel
{
    /// <summary>
    /// Base implementation of ICommand.  Enable and Disable methods change the CanExecute
    /// methods output.
    /// </summary>
    public class ButtonExecute : ICommand
    {
        private Action _RemoteAction;
        private bool _canExecute;

        public ButtonExecute(Action RemoteAction)
        {
            _RemoteAction = RemoteAction;
            _canExecute = false;
        }

        public void Enable()
        {
            _canExecute = true;
            ExecuteChanged();
        }

        public void Disabled()
        {
            _canExecute = false;
            ExecuteChanged();
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public event EventHandler CanExecuteChanged;

        private void ExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, null);
        }
        public void Execute(object parameter)
        {
            _RemoteAction();
        }
    }
}
