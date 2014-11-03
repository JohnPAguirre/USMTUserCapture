using _335thUserCapture.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _335thUserCapture.ViewModel.CaptureOneUserOnComputer
{
    public class ComputerUserViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<string> _users;
        private IUserSelected _selectedUser;

        public ComputerUserViewModel(IUsersInfo allUsers, IUserSelected selectedUser)
        {
            _users = new ObservableCollection<string>(allUsers.AllUsers);
            _selectedUser = selectedUser;
            //_selectedUser.SelectedUser = _users[0];
        }

        public string SelectedUser
        {
            set
            {
                _selectedUser.SelectedUser = value;
                _selectedUser.IsUserValid = true;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedUser"));
                }
            }

            get
            {
                if (_selectedUser == null)
                    return null;
                return _selectedUser.SelectedUser;
            }
        }

        public ObservableCollection<string> Users
        {
            get
            {
                return _users;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
