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
    /// Interaction logic for ListUsers.xaml
    /// </summary>
    public partial class ListUsers : UserControl
    {
        public ListUsers()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception)
            {
                
                MessageBox.Show("XAML Error in the List Users");
                Application.Current.Shutdown();
            }
            DataContext = ViewModelFactory.CreateComputerUserViewModel; ;
        }
    }
}
