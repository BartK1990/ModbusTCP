﻿using System;
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
        string lastIpAddressPart;
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

        private void IPInputHandling(object sender, TextBox nextTb, TextCompositionEventArgs e)
        {
            if (e.Text == "." && nextTb != null)
                nextTb.Focus();
            e.Handled = !((int.TryParse(e.Text, out int i)) && IPPartInRange(i)); // checks if input is number 0...255
            if (sender is TextBox)
            {
                TextBox tb = sender as TextBox;
                lastIpAddressPart = tb.Text;
            }
        }

        private bool IPPartInRange(int i)
        {
            return (i >= 0 && i <= 255) ? true : false;
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
            IPInputHandling(sender, IPAddr2, e);
        }

        private void IPAddr2_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            IPInputHandling(sender, IPAddr3, e);
        }

        private void IPAddr3_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            IPInputHandling(sender, IPAddr4, e);
        }

        private void IPAddr4_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            IPInputHandling(sender, null, e);
        }

        private void IPPort_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !((int.TryParse(e.Text, out int i)) && i >= 0 && i <= 65536); // checks if input is number 0...65536
        }

        private void IPAddr1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(sender is TextBox)
            {
                TextBox tb = sender as TextBox;
                if (int.TryParse((tb.Text), out int i))
                    if(!IPPartInRange(i))
                        tb.Text = lastIpAddressPart;
            }      
        }
    }
}
