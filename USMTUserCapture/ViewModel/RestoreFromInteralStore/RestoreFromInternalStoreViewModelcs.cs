using USMTUserCapture.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace USMTUserCapture.ViewModel.RestoreFromInteralStore
{
    /// <summary>
    /// View model to the restore user tab
    /// </summary>
    public class RestoreFromInternalStoreViewModel : INotifyPropertyChanged
    {
        private List<IUserJob> _allBackupJobs;
        private IUserJob _selectedBackupJob;
        private ButtonAsyncExecute _startBackup;
        private ButtonAsyncExecute _deleteJob;
        private string _output;

        /// <summary>
        /// Displays all user jobs retrieved from IGetBackupInformation (the database)
        /// </summary>
        public List<IUserJob> AllBackupJobs
        {
            get
            {
                return _allBackupJobs;
            }
        }

        /// <summary>
        /// Updates selected backup job. This will pass to the restore system once StartBackup is executed.  Once set, it enables the start backup button
        /// </summary>
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
                _deleteJob.Enable();
            }
        }

        /// <summary>
        /// Implements the button.  This is initially disabled until and SelectedBackupJob has a entry.
        /// </summary>
        public ICommand StartRestore
        {
            get
            {
                return _startBackup;
            }
        }

        public ICommand DeleteJob
        {
            get
            {
                return _deleteJob;
            }
        }

        /// <summary>
        /// This will have to output provided by the ILoadState stream returned by the ILoadState StartRestore method.  
        /// Updated by the function put into the button.
        /// </summary>
        public string Output
        {
            get
            {
                return _output;
            }
        }

        /// <summary>
        /// Constructor to initialize componenets. Sets up button function and retrievesall the backups in
        /// IGetGackupInformation (databasee) alongwith the ILoadState restore interface.
        /// </summary>
        /// <param name="db">All Users backed up</param>
        /// <param name="restore">The restore mechanism</param>
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
            //stop messing with the async code, its sorta funky
            _startBackup = new ButtonAsyncExecute( async ()=>{
                var stream = restore.StartRestore(_selectedBackupJob);
                _startBackup.DisableForever();
                await Task.Run(async () =>
                {
                    char[] temp = new char[1];
                    while ((await stream.ReadAsync(temp, 0, 1) != 0))
                    {

                        _output += temp[0];
                        ChangedProperty("Output");
                    }
                });

            });
            _deleteJob = new ButtonAsyncExecute(async () =>
            {
                db.DeleteBackup(_selectedBackupJob);
                _allBackupJobs = db.AllBackups();
                ChangedProperty("AllBackupJobs");
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
