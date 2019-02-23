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

namespace ModbusTCP.UserControls
{
    /// <summary>
    /// Interaction logic for IPPortTextBox.xaml
    /// </summary>
    public partial class IPPortTextBox : UserControl
    {
        string lastPort;
        private readonly TextBox portTextBox;

        public IPPortTextBox()
        {
            InitializeComponent();
            portTextBox = IPPort;
        }

        public string Port
        {
            get { return (string)GetValue(PortProperty); }
            set { SetValue(PortProperty, value); }
        }
        public static readonly DependencyProperty PortProperty =
            DependencyProperty.Register("Port", typeof(string), typeof(IPPortTextBox), new FrameworkPropertyMetadata(default(string), PortChanged)
            {
                BindsTwoWayByDefault = true
            });

        private static void PortChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var ipPortTextBox = dependencyObject as IPPortTextBox;
            var text = e.NewValue as string;
            if (text != null && ipPortTextBox != null)
                ipPortTextBox.portTextBox.Text = text;
        }

        #region Input handling

        private bool PortInRange(int i) // checks if input is number 0...65536
        {
            return (i >= 0 && i <= 65536) ? true : false;
        }

        private void IPPort_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !((int.TryParse(e.Text, out int i)) && PortInRange(i)); 
            if (sender is TextBox)
            {
                TextBox tb = sender as TextBox;
                lastPort = tb.Text;
            }
            //Port = IPPort.Text;
        }

        private void IPPort_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox tb = sender as TextBox;
                if (int.TryParse((tb.Text), out int i))
                {
                    if (!PortInRange(i))
                    {
                        tb.Text = lastPort;
                        tb.SelectionStart = tb.Text.Length;
                    }
                }
            }
            Port = IPPort.Text;
        }

        #endregion
    }
}
