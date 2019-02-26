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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MBConnectView mBConnectView;
        private LoggerView loggerView;

        public MainWindow()
        {
            InitializeComponent();
            mBConnectView = new MBConnectView();
            loggerView = new LoggerView();

        }

        private void MenuConnect_button_Click(object sender, RoutedEventArgs e)
        {
            WindowContent.Content = mBConnectView;
        }

        private void MenuLogger_button_Click(object sender, RoutedEventArgs e)
        {
            WindowContent.Content = loggerView;
        }
    }
}
