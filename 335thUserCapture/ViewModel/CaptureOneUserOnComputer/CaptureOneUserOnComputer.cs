using _335thUserCapture.Interfaces;
using _335thUserCapture.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace _335thUserCapture.ViewModel.CaptureOneUserOnComputer
{
    public class CaptureOneUserOnComputerViewModel : INotifyPropertyChanged
    {
        private List<string> _users;
        private string _selectedUser;
        private IFolderInformation _folders;
        private ISaveBackupInformation _db;
        private StringBuilder _output;

        private ButtonAsyncExecute _go;
        
        /// <summary>
        /// All of the users in the C:\Users folder is displayed here
        /// </summary>
        public List<string> Users
        {
            get
            {
                return _users;
            }
        }

        public string SelectedUser
        {
            get
            {
                return _selectedUser;
            }
            set
            {
                _selectedUser = value;
                PropertyModified("SelectedUser");
                this.CheckButtonConditional();
            }
        }


        public string Location
        {
            get
            {
                return _folders.BaseFolder + _folders.UserBackupFolder;
            }
        }

        public ICommand StartBackup
        {
            get
            {
                return _go;
            }
        }

        public string Output
        {
            get
            {
                return _output.ToString();
            }
        }

        public CaptureOneUserOnComputerViewModel(
            IUsersInfo allUsers, 
            IFolderInformation folders,
            ISaveBackupInformation db)
        {
            _users = new List<string>(allUsers.AllUsers);
            _folders = folders;
            _db = db;

            _output = new StringBuilder();
            _go = new ButtonAsyncExecute(Start);
        }

        public async Task Start()
        {
            //Save our job
            int ID = _db.SaveBackupInfo(this.SelectedUser, Environment.GetEnvironmentVariable("COMPUTERNAME"), _folders.UserBackupFolder);
            //create our folder
            _folders.CreateUserBackupFolder();

            //start backup and reflect change inside of window
            ScanState backup = new ScanState(SelectedUser, _folders);

            StreamReader output = backup.Output;
            char[] temp = new char[1];
            while ((await output.ReadAsync(temp, 0, 1) != 0))
            {
                _output.Append(temp[0]);
                PropertyModified("Output"); //
            }

            //Annotate that our job is done
            _db.CompletedBackup(ID);
        }

        private void CheckButtonConditional()
        {
            if (SelectedUser != "" &&
                _folders.IsBaseFolderValid)
                _go.Enable();
            else
                _go.Disabled();

        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void PropertyModified(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

    }
}
