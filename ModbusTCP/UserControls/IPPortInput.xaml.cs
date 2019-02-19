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

namespace ModbusTCP.UserControls
{
    /// <summary>
    /// Interaction logic for IPPortInput.xaml
    /// </summary>
    public partial class IPPortInput : UserControl
    {
        public IPPortInput()
        {
            InitializeComponent();
        }

        private void IPPort_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !((int.TryParse(e.Text, out int i)) && i >= 0 && i <= 65536); // checks if input is number 0...65536
        }

        public String Text
        {
            get { return IPPort.Text; }
        }
    }
}
