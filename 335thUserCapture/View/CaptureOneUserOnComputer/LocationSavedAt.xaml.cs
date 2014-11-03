using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using _335thUserCapture.ViewModel;
using _335thUserCapture.ObjectFactory;

namespace _335thUserCapture.View.CaptureOneUserOnComputer
{
    /// <summary>
    /// Interaction logic for LocationSavedAt.xaml
    /// </summary>
    public partial class LocationSavedAt : UserControl
    {
        public LocationSavedAt()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception)
            {
                
                MessageBox.Show("XAML Error in the LocationSavedAt");
                Application.Current.Shutdown();
            }
            DataContext = ViewModelFactory.CreateLocationViewModel;
        }


        //was allowing user to select where to dump it.  Currently disallowing this feature
        /*
        private void OpenBrowse(object sender, RoutedEventArgs e)
        {
            var folder = new System.Windows.Forms.FolderBrowserDialog();
            folder.ShowNewFolderButton = true;
            folder.Description = "Please select a folder to save the users data";
            var result = folder.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                ((LocationViewModel)DataContext).Location = folder.SelectedPath;
            }
        }
        */
    }
}
