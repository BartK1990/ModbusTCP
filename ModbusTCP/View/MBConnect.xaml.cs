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
using System.Windows.Shapes;

namespace ModbusTCP.View
{
    /// <summary>
    /// Interaction logic for MBConnect.xaml
    /// </summary>
    public partial class MBConnect : Window
    {
        public MBConnect()
        {
            InitializeComponent();
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            //mbtcpConnManager.ConnectAsync();
        }

        private void IPPort_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }
    }
}