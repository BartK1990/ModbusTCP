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


        public IPPortTextBox()
        {
            InitializeComponent();
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
                ipPortTextBox._segments[i].Text = segment;
        }

        #region Input handling

        private void IPPort_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !((int.TryParse(e.Text, out int i)) && i >= 0 && i <= 65536); // checks if input is number 0...65536
            Port = IPPort.Text;
        }

        #endregion
    }
}
