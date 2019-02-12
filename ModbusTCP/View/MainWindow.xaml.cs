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
        string lastIpAddressPart;
        MBTCPConnManager mbtcpConnManager;

        public MainWindow()
        {
            mbtcpConnManager = new MBTCPConnManager();
            InitializeComponent();
        }

        private void IPInputPreviewTextInputHandling(object sender, TextBox nextTb, TextCompositionEventArgs e)
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

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            mbtcpConnManager.ConnectAsync();
        }

        private void SetIP_Click(object sender, RoutedEventArgs e)
        {
            string ipAddr = IPAddr1.Text + '.' + IPAddr2.Text + '.' + IPAddr3.Text + '.' + IPAddr4.Text;
            mbtcpConnManager.SetSlaveIPAddrAndPort(ipAddr, IPPort.Text, out string log);

            LoggerManager.LogToMainWindow(log);
        }

        #region IPInputHandling

        private void IPInputTectChangedHandling(object sender)
        {
            if (sender is TextBox)
            {
                TextBox tb = sender as TextBox;
                if (int.TryParse((tb.Text), out int i))
                {
                    if (!IPPartInRange(i))
                    {
                        tb.Text = lastIpAddressPart;
                        tb.SelectionStart = tb.Text.Length;
                    }
                }
            }
        }

        private bool IPPartInRange(int i)
        {
            return (i >= 0 && i <= 255) ? true : false;
        }

        private void IPAddr1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            IPInputPreviewTextInputHandling(sender, IPAddr2, e);
        }

        private void IPAddr2_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            IPInputPreviewTextInputHandling(sender, IPAddr3, e);
        }

        private void IPAddr3_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            IPInputPreviewTextInputHandling(sender, IPAddr4, e);
        }

        private void IPAddr4_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            IPInputPreviewTextInputHandling(sender, null, e);
        }

        private void IPPort_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !((int.TryParse(e.Text, out int i)) && i >= 0 && i <= 65536); // checks if input is number 0...65536
        }

        private void IPAddr1_TextChanged(object sender, TextChangedEventArgs e)
        {
            IPInputTectChangedHandling(sender);
        }

        private void IPAddr2_TextChanged(object sender, TextChangedEventArgs e)
        {
            IPInputTectChangedHandling(sender);
        }

        private void IPAddr3_TextChanged(object sender, TextChangedEventArgs e)
        {
            IPInputTectChangedHandling(sender);
        }

        private void IPAddr4_TextChanged(object sender, TextChangedEventArgs e)
        {
            IPInputTectChangedHandling(sender);
        }
        #endregion 
    }
}
