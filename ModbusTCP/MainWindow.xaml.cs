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
        struct LoggerItem
        {
            public string Log {get; set;}
            public string Time { get; set; }
            public LoggerItem(string log)
            {
                Log = log;
                Time = DateTime.Now.ToString("h:mm:ss tt");
            }
        }

        public MainWindow()
        {
            InitializeComponent();

        }

        private void SetIP_Click(object sender, RoutedEventArgs e)
        {
            MBTCPConnManager mbtcpConnManager = new MBTCPConnManager();
            string ipAddr = IPAddr1.Text + '.' + IPAddr2.Text + '.' + IPAddr3.Text + '.' + IPAddr4.Text;
            mbtcpConnManager.SetSlaveIPAddrAndPort(ipAddr, int.Parse(IPPort.Text) , out string log);

            LoggerAdd(log);
        }

        private void LoggerAdd(string info)
        {
            Logger.Items.Insert(0, new LoggerItem(info));
        }


    }
}
