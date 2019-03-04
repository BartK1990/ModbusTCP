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
        private MBConnectView mbConnectView;
        private LoggerView loggerView;
        private CommunicationView communicationView;

        private Model.WindowLogger windowLogger;
        private Model.MBTCPConn mbTCPConn;

        public MainWindow()
        {
            InitializeComponent();
            windowLogger = new Model.WindowLogger();
            mbTCPConn = new Model.MBTCPConn(windowLogger);

            mbConnectView = new MBConnectView();
            loggerView = new LoggerView();
            communicationView = new CommunicationView();

            this.DataContext = new ViewModel.MBConnectViewModel(mbTCPConn, windowLogger);
            mbConnectView.DataContext = this.DataContext;
            loggerView.DataContext = new ViewModel.LoggerViewModel(windowLogger);
            communicationView.DataContext = new ViewModel.CommunicationViewModel(mbTCPConn);
        }

        private void MenuConnect_button_Click(object sender, RoutedEventArgs e)
        {
            WindowContent.Content = mbConnectView;
        }

        private void MenuLogger_button_Click(object sender, RoutedEventArgs e)
        {
            WindowContent.Content = loggerView;
        }

        private void MenuCommunication_button_Click(object sender, RoutedEventArgs e)
        {
            WindowContent.Content = communicationView;
        }
    }
}
