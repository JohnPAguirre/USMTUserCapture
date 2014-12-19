using USMTUserCapture.ViewModel.CaptureOneUserOnComputer;
using USMTUserCapture.Model;
using USMTUserCapture.Interfaces;
using USMTUserCapture.ViewModel.RestoreFromInteralStore;


namespace USMTUserCapture.ObjectFactory
{
    /// <summary>
    /// This class creates all revelent ViewModels for the views that reference this model
    /// Since all classes have to reference each other, we have to create a singleton of most of them and have the dependency resolved.
    /// </summary>
    public class ViewModelFactory
    {
        private static RestoreFromInternalStoreViewModel _restoreFromInternalStoreViewModel;
        private static CaptureOneUserOnComputerViewModel _captureOneUserOnComputerViewModel;

        static public CaptureOneUserOnComputerViewModel CreateCaptureOneUserOnComputerViewModel
        {
            get
            {
                if (_captureOneUserOnComputerViewModel == null)
                    _captureOneUserOnComputerViewModel = new CaptureOneUserOnComputerViewModel(
                        ModelFactory.CreateUserList,
                        ModelFactory.CreateBaseLocationsFolder,
                        ModelFactory.CreatWriteDB);
                return _captureOneUserOnComputerViewModel;
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
