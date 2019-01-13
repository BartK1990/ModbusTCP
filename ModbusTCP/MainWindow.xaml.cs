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

namespace ModbusTCP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        MBTCPConnManager mbtcpConnManager;

        public MainWindow()
        {
            InitializeComponent();

        }

        struct LoggerItem
        {
            public string Log { get; set; }
            public string Time { get; set; }
            public LoggerItem(string log)
            {
                Log = log;
                Time = DateTime.Now.ToString("h:mm:ss tt");
            }
        }

        public static bool IsValid(string str)
        {
            int i;
            return int.TryParse(str, out i) && i >= 5 && i <= 9999;
        }

        private void SetIP_Click(object sender, RoutedEventArgs e)
        {
            mbtcpConnManager = new MBTCPConnManager();
            string ipAddr = IPAddr1.Text + '.' + IPAddr2.Text + '.' + IPAddr3.Text + '.' + IPAddr4.Text;
            mbtcpConnManager.SetSlaveIPAddrAndPort(ipAddr, IPPort.Text, out string log);

            LoggerAdd(log);
        }

        private void LoggerAdd(string info)
        {
            Logger.Items.Insert(0, new LoggerItem(info));
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            Connect_MBTCP();
        }

        private async void Connect_MBTCP()
        {
            Logger.Items.Insert(0, new LoggerItem(await mbtcpConnManager.ConnectAsync()));
        }

        private void IPAddr1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsValid(((TextBox)sender).Text + e.Text);
        }
    }
}
