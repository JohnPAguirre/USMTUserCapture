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
    /// <summary>
    /// Please see IFolderInformation for implementation details
    /// </summary>
    public class BaseInformation : IFolderInformation
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
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("BaseFolder"));
                    PropertyChanged(this, new PropertyChangedEventArgs("IsBaseFolderValid"));
                }


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



        private bool _isBaseFolderValid;
        public bool IsBaseFolderValid
        {
            get
            {
                return (new DirectoryInfo(this._baseFolder)).Exists;
            }
        }

        public BaseInformation() 
        {
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
