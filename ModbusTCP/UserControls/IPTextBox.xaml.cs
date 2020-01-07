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
    /// Interaction logic for IPInput.xaml
    /// </summary>
    public partial class IPTextBox : UserControl
    {
        string lastIpAddressPart;

        private readonly List<TextBox> _segments = new List<TextBox>();

        public IPTextBox()
        {
            InitializeComponent();
            _segments.Add(IPAddr1);
            _segments.Add(IPAddr2);
            _segments.Add(IPAddr3);
            _segments.Add(IPAddr4);
        }
            
        public string Address
        {
            get => (string)GetValue(AddressProperty);
            set => SetValue(AddressProperty, value);
        }
        public static readonly DependencyProperty AddressProperty =
           DependencyProperty.Register("Address", typeof(string), typeof(IPTextBox), new FrameworkPropertyMetadata(default(string), AddressChanged)
           {
               BindsTwoWayByDefault = true
           });

        private static void AddressChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var ipTextBox = dependencyObject as IPTextBox;
            var text = e.NewValue as string;
            string[] segments = new string[4];

            if (text != null && ipTextBox != null)
            {
                var i = 0;
                foreach (var segment in text.Split('.'))
                {
                    ipTextBox._segments[i].Text = segment;
                    i++;
                }
            }
        }

        #region IPInputHandling

        private string ReturnAddress()
        {
            return IPAddr1.Text + '.' + IPAddr2.Text + '.' + IPAddr3.Text + '.' + IPAddr4.Text;
        }

        private void IPTextBoxPreviewTextInputHandling(object sender, TextBox nextTb, TextCompositionEventArgs e)
        {
            if (e.Text == "." && nextTb != null)
                nextTb.Focus();
            e.Handled = !((int.TryParse(e.Text, out int i)) && IPPartInRange(i)); // checks if input is number 0...255
            if (sender is TextBox)
            {
                TextBox tb = sender as TextBox;
                lastIpAddressPart = tb.Text;
            }
            //Address = ReturnAddress();
        }

        private void IPTextBoxTextChangedHandling(object sender)
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
            Address = ReturnAddress();
        }

        private bool IPPartInRange(int i)
        {
            return (i >= 0 && i <= 255) ? true : false;
        }

        private void IPAddr1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            IPTextBoxPreviewTextInputHandling(sender, IPAddr2, e);
        }

        private void IPAddr2_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            IPTextBoxPreviewTextInputHandling(sender, IPAddr3, e);
        }

        private void IPAddr3_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            IPTextBoxPreviewTextInputHandling(sender, IPAddr4, e);
        }

        private void IPAddr4_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            IPTextBoxPreviewTextInputHandling(sender, null, e);
        }

        private void IPAddr1_TextChanged(object sender, TextChangedEventArgs e)
        {
            IPTextBoxTextChangedHandling(sender);
        }

        private void IPAddr2_TextChanged(object sender, TextChangedEventArgs e)
        {
            IPTextBoxTextChangedHandling(sender);
        }

        private void IPAddr3_TextChanged(object sender, TextChangedEventArgs e)
        {
            IPTextBoxTextChangedHandling(sender);
        }

        private void IPAddr4_TextChanged(object sender, TextChangedEventArgs e)
        {
            IPTextBoxTextChangedHandling(sender);
        }
        #endregion 
    }
}
