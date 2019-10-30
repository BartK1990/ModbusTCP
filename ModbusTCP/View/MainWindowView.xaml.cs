using System;
using System.Collections.Generic;
using System.Globalization;
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
        private readonly MBConnectView mbConnectView;
        private readonly LoggerView loggerView;
        private readonly CommunicationView communicationView;
        
        public MainWindow()
        {
            InitializeComponent();

            // Initialize Model instances for Window
            Model.WindowLogger windowLogger = new Model.WindowLogger();
            Model.MBTCPConn mbTCPConn = new Model.MBTCPConn(windowLogger);

            // Initialize views for Window
            mbConnectView = new MBConnectView();
            loggerView = new LoggerView();
            communicationView = new CommunicationView();

            // Initialize ViewModel and assign to DataContext for window
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
