using _335thUserCapture.ObjectFactory;
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

namespace _335thUserCapture.View.CaptureOneUserOnComputer
{
    /// <summary>
    /// Interaction logic for Output.xaml
    /// </summary>
    public partial class Output : UserControl
    {
        public Output()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception)
            {
                
                MessageBox.Show("XAML Error in the Output");
                Application.Current.Shutdown();
            }
            DataContext = ViewModelFactory.CreateOutputViewModel;
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            ((TextBox)sender).ScrollToEnd();
        }
    }
}
