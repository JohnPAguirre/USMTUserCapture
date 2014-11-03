using _335thUserCapture.Interfaces;
using System.ComponentModel;
using System.IO;

namespace _335thUserCapture.ViewModel.CaptureOneUserOnComputer
{
    /// <summary>
    /// This class gets and set the default directory to save
    /// </summary>
    public class LocationViewModel : INotifyPropertyChanged
    {
        private IFolderInformation baseFolders;

        public LocationViewModel(IFolderInformation Location)
        {
            baseFolders = Location;
        }
        public string Location
        {
            set
            {
                baseFolders.UserBackupFolder = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Location"));
                }
                SetLocationValid();

            }
            get
            {
                return baseFolders.BaseFolder + baseFolders.UserBackupFolder;
            }
        }

        public void SetLocationValid(){
            if ((new DirectoryInfo(baseFolders.UserBackupFolder)).Exists)
            {
                baseFolders.IsBaseFolderValid = true;
            }
            else
            {
                baseFolders.IsBaseFolderValid = false;
            }

        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
