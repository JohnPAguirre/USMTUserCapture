using _335thUserCapture.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace _335thUserCapture.ViewModel.RestoreFromInteralStore
{
    public class RestoreFromInternalStoreViewModel : INotifyPropertyChanged
    {
        private List<IUserJob> _allBackupJobs;
        private IUserJob _selectedBackupJob;
        private ButtonExecute _startBackup;
        private string _output;

        public List<IUserJob> AllBackupJobs
        {
            get
            {
                return _allBackupJobs;
            }
        }

        public IUserJob SelectedBackupJob
        {
            get
            {
                return _selectedBackupJob;
            }
            set
            {
                _selectedBackupJob = value;
                ChangedProperty("SelectedBackupJob");
                _startBackup.Enable();
            }
        }

        public ICommand StartBackup
        {
            get
            {
                return _startBackup;
            }
        }

        public string Output
        {
            get
            {
                return _output;
            }
            set
            {
                _output = value;
                ChangedProperty("Output");
            }
        }

        public RestoreFromInternalStoreViewModel(IGetBackupInformation db, ILoadState restore)
        {
            try
            {
                _allBackupJobs = db.AllBackups();
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong with retrieving all the backups");
                Application.Current.Shutdown();
            }
            _output = "";
            _startBackup = new ButtonExecute(()=>{
                var stream = restore.StartRestore(_selectedBackupJob);
                _startBackup.Disabled();
                Task.Run(async () =>
                {
                    char[] temp = new char[1];
                    while ((await stream.ReadAsync(temp, 0, 1) != 0))
                    {

                        _output += temp[0];
                        ChangedProperty("Output");
                    }
                });
            });
        }


        public event PropertyChangedEventHandler PropertyChanged;
        
        /// <summary>
        /// Will fire off PropertyChanged and handle any housekeeping
        /// </summary>
        /// <param name="propertyName">Name of the property that was changed</param>
        private void ChangedProperty(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }


}
