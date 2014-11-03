using _335thUserCapture.Interfaces;
using _335thUserCapture.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace _335thUserCapture.ViewModel.CaptureOneUserOnComputer
{
    /// <summary>
    /// ViewModel for StartBackup functionality
    /// </summary>
    public class StartBackupViewModel : INotifyPropertyChanged
    {
        private IUserSelected _userSelected;
        private IFolderInformation _folders;

        private ExecuteAction _go;
        public ICommand Go
        {
            get
            {
                return _go;
            }

        }

        /// <summary>
        /// Gets state from the Computer user and Location, enables or disables button and executes onces validated
        /// </summary>
        /// <param name="computerUserVM"></param>
        /// <param name="locationVM"></param>
        /// <remarks>TODO: Refactor PropertyChanged</remarks>
        public StartBackupViewModel(IUserSelected UserSelected, IFolderInformation folders, IRemoteExecute StartBackup)
        {
            _userSelected = UserSelected;
            _folders = folders;
            _go = new ExecuteAction(StartBackup.Start);

            //if the property changes on either, our possible isValid has changed state
            _userSelected.PropertyChanged += ((object value, PropertyChangedEventArgs e) =>
            {
                Validate();
            });
            _folders.PropertyChanged += ((object value, PropertyChangedEventArgs e) =>
            {
                Validate();
            });
        }

        public void Validate()
        {
            if (_userSelected.IsUserValid == true &&
                _folders.IsBaseFolderValid == true)
                _go.isValid = true;
            else
                _go.isValid = false;
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Test()
        {
            
        }
    }

    class ExecuteAction : ICommand
    {
        private Action _stuff;
        private bool _isValid;
        //once the user hits go once, disable the button for the rest of the program
        private bool disable;
        public bool isValid 
        {
            get 
            {
                return _isValid;
            }
            set
            {
                _isValid = value;
                if (CanExecuteChanged != null)
                {
                    CanExecuteChanged(this, new PropertyChangedEventArgs("isValid"));
                    CanExecuteChanged(this, new PropertyChangedEventArgs("CanExecute"));
                }
            }
        }
        public ExecuteAction(Action execute)
        {
            _stuff = execute;
            isValid = false;
            disable = false;
        }


        public bool CanExecute(object parameter)
        {
            if (disable)
                return false;
            return isValid;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _stuff();
            disable = true;
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, new PropertyChangedEventArgs("CanExecute"));
        }
    }


}
