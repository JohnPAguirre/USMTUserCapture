using _335thUserCapture.Interfaces;
using _335thUserCapture.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _335thUserCapture.ViewModel.CaptureOneUserOnComputer
{
    public class OutputViewModel : INotifyPropertyChanged, IRemoteExecute
    {
        private IFolderInformation _folders;
        private IUserSelected _user;
        ISaveBackupInformation _db;

        public OutputViewModel(IFolderInformation folders, IUserSelected user, ISaveBackupInformation db)
        {
            _folders = folders;
            _user = user;
            _db = db;
        }
        //current values inside of scanstate
        private string _output;
        public string Output
        {
            get
            {
                return _output;
            }
            set
            {
                _output = value;
                if (this.PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Output"));
            }
        }

        /// <summary>
        /// starts the backup process
        /// TODO: Still working on it.
        /// </summary>
        /// <param name="user"></param>
        public void StartBackup()
        {
            int ID = _db.SaveBackupInfo(_user.SelectedUser, Environment.GetEnvironmentVariable("COMPUTERNAME"), _folders.UserBackupFolder);
            //start backup and reflect change inside of window
            _folders.CreateUserBackupFolder();
            ScanState backup = new ScanState(_user.SelectedUser, _folders);

            //The ReadAsync from the streamreader locks up our GUI so we have to run it in its own process.  
            //This variable holds a function that holds a process that executes a function
            //TODO: I really really need to refactor this to get it slightly less complex but it works for the moment.
            //Possibly move this complication to inside of the scanstate class and just signal the GUI when there is a update avaliable
            var DoingLotsOfStuff = new Action(async () =>
            {
                await Task.Run(
                    (Action)(async ()=>{
                                
                                await backup.Ready();
                                StreamReader output = backup.Output;
                                char[] temp = new char[1];
                                while ((await output.ReadAsync(temp, 0, 1) != 0))
                                    this.Output += temp[0];
                }));
            });
            DoingLotsOfStuff();

            _db.CompletedBackup(ID);
        }

        public Action Start 
        {
            get
            {
                return new Action(this.StartBackup);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
