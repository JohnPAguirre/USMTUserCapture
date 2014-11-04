using _335thUserCapture.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _335thUserCapture.ViewModel.CaptureOneUserOnComputer
{
    class CaptureOneUserOnComputer : INotifyPropertyChanged
    {
        private List<string> _users;
        private string _selectedUser;
        private IFolderInformation _folders;
        private ISaveBackupInformation _db;
        private StringBuilder _output;

        private ButtonExecute _go;

        public CaptureOneUserOnComputer(IUsersInfo allUsers, 
            IFolderInformation folders,
            ISaveBackupInformation db)
        {
            _users = new List<string>(allUsers.AllUsers);
            _folders = folders;
            _db = db;

            _output = new StringBuilder();
            _go = new ButtonExecute(() =>
            {
                //stuff happens here when the button is activated
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void PropertyModified(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
