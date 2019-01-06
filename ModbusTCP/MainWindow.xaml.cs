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
        }

        public MainWindow()
        {
            InitializeComponent();




        }

        private void SetIP_Click(object sender, RoutedEventArgs e)
        {
            MBTCPConnManager mbtcpConnManager = new MBTCPConnManager();
            if (mbtcpConnManager.SetSlaveIPAddr(IPAddress.Text))
                Logger.Items.Add(new LoggerItem() { Log = "IP address Set" });
            else
                Logger.Items.Add(new LoggerItem() { Log = "Wrong IP address format" });
        }
    }
}
