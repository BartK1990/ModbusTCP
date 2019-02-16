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
using System.Windows.Shapes;

namespace ModbusTCP.View
{
    /// <summary>
    /// Interaction logic for MBConnect.xaml
    /// </summary>
    public partial class MBConnect : Window
    {
        string lastIpAddressPart;
        //MBTCPConnManager mbtcpConnManager;

        public MBConnect()
        {
            InitializeComponent();
        }

        private void IPPort_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !((int.TryParse(e.Text, out int i)) && i >= 0 && i <= 65536); // checks if input is number 0...65536
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            //mbtcpConnManager.ConnectAsync();
        }

        private void SetIP_Click(object sender, RoutedEventArgs e)
        {

            string ipAddr = IPInputBox.Text;
            //mbtcpConnManager.SetSlaveIPAddrAndPort(ipAddr, IPPort.Text, out string log);

            //LoggerManager.LogToMainWindow(log);
        }

    }
}
