using _335thUserCapture.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _335thUserCapture.Model
{
    public class BaseInformation : IFolderInformation, IUserSelected
    {
        //IFolderInformation
        private string _baseFolder;
        public string BaseFolder {
            get 
            {
                return _baseFolder;
            }
            set
            {
                _baseFolder = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("BaseFolder"));

            }
        }
        private string _USMTBinaryFolder;
        public string USMTBinaryFolder {
            get 
            {
                return _USMTBinaryFolder;
            }
            set
            {
                _USMTBinaryFolder = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("USMTBinaryFolder"));

            }
        }
        private string _UserBackupFolder;
        public string UserBackupFolder {
            get
            {
                return _UserBackupFolder;
            }
            set
            {
                _UserBackupFolder = value;
                this.LogFilesFolder = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("UserBackupFolder"));

            }
        }
        private string _LogFilesFolder;
        public string LogFilesFolder {
            get
            {
                return _LogFilesFolder;
            }
            set
            {
                _LogFilesFolder = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("LogFilesFolder"));

            }
        }

        //IUserSelected
        private string _selectedUser;
        public string SelectedUser {
            get
            {
                return _selectedUser;
            }
            set
            {
                _selectedUser = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedUser"));

            }
        }


        private bool _isUserValid;
        /// <summary>
        /// This value needs to be set when a user is selected.
        /// </summary>
        public bool IsUserValid
        {
            get
            {
                return _isUserValid;
            }
            set 
            {
                _isUserValid = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("IsUserValid"));

            }
        }

        private bool _isBaseFolderValid;
        public bool IsBaseFolderValid
        {
            get
            {
                return _isBaseFolderValid;
            }
            set 
            {
                _isBaseFolderValid = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("IsBaseFolderValid"));
            }
        }

        public BaseInformation() 
        {
            _isUserValid = false;
            _isBaseFolderValid = false;

            //original method to get file path was a bad way to do it
            //BaseFolder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            //new way works much better
            BaseFolder = System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);

            USMTBinaryFolder = @"\USMT\binary\";
            UserBackupFolder = @"\UserBackups\" + DateTime.Now.Year + 
                DateTime.Now.Month + 
                DateTime.Now.Day + "." +
                DateTime.Now.Hour +
                DateTime.Now.Minute + @"\";
            LogFilesFolder = @"\UserBackups\" + DateTime.Now.Year +
                DateTime.Now.Month +
                DateTime.Now.Day + "." +
                DateTime.Now.Hour + 
                DateTime.Now.Minute  + @"\";
            _isBaseFolderValid = true;

        }

        public event PropertyChangedEventHandler PropertyChanged;


        public void CreateUserBackupFolder()
        {
            DirectoryInfo userFolder = new DirectoryInfo(_baseFolder + _UserBackupFolder);
            if (!userFolder.Exists)
                userFolder.Create();
        }
    }
}
