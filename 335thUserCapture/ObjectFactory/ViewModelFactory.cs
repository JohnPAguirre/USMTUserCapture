using _335thUserCapture.ViewModel.CaptureOneUserOnComputer;
using _335thUserCapture.Model;
using _335thUserCapture.Interfaces;
using _335thUserCapture.ViewModel.RestoreFromInteralStore;


namespace _335thUserCapture.ObjectFactory
{
    /// <summary>
    /// This class creates all revelent ViewModels for the views that reference this model
    /// Since all classes have to reference each other, we have to create a singleton of most of them and have the dependency resolved.
    /// </summary>
    public class ViewModelFactory
    {
        private static ComputerUserViewModel _computerUserViewModel;
        private static LocationViewModel _locationViewModel;
        private static StartBackupViewModel _startBackupViewModel;
        private static OutputViewModel _outputViewModel;
        private static RestoreFromInternalStoreViewModel _restoreFromInternalStoreViewModel;

        static public ComputerUserViewModel CreateComputerUserViewModel
        {
            get
            {
                if (_computerUserViewModel == null)
                    _computerUserViewModel = new ComputerUserViewModel(ModelFactory.CreateUserList,
                        ModelFactory.CreateUserSelected);
                return _computerUserViewModel;
            }
        }

        static public LocationViewModel CreateLocationViewModel
        {
            get
            {
                if (_locationViewModel == null)
                    _locationViewModel = new LocationViewModel(ModelFactory.CreateBaseLocationsFolder);
                return _locationViewModel;
            }
        }

        static public StartBackupViewModel CreateStartBackupViewModel
        {
            get
            {
                if (_startBackupViewModel == null)
                    _startBackupViewModel = new StartBackupViewModel(ModelFactory.CreateUserSelected,
                        ModelFactory.CreateBaseLocationsFolder,
                        ViewModelFactory.CreateOutputViewModel);
                return _startBackupViewModel;
            }
        }

        static public OutputViewModel CreateOutputViewModel
        {
            get
            {
                if (_outputViewModel == null)
                {
                    _outputViewModel = new OutputViewModel(ModelFactory.CreateBaseLocationsFolder,
                        ModelFactory.CreateUserSelected,
                        ModelFactory.CreatWriteDB);
                }
                return _outputViewModel;
            }

        }

        static public RestoreFromInternalStoreViewModel CreateRestoreFromInternalStoreViewModel
        {
            get
            {
                if (_restoreFromInternalStoreViewModel == null)
                {
                    _restoreFromInternalStoreViewModel = new RestoreFromInternalStoreViewModel(ModelFactory.CreateReadDB,ModelFactory.CreateLoadState);
                }
                return _restoreFromInternalStoreViewModel;
            }
        }
    }
}
