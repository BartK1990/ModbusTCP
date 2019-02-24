using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusTCP.ViewModel
{
    using Model;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;

    public struct LoggerItem
    {
        public string Log { get; set; }
        public string Time { get; set; }
        public LoggerItem(string log)
        {
            Log = log;
            Time = DateTime.Now.ToString();
        }
    }

    public class MBConnectViewModel : INotifyPropertyChanged
    {
        private MBTCPConn _mbtcpconn = new MBTCPConn();

        private string iPAddressText;
        public string IPAddressText
        {
            get { return iPAddressText; }
            set
            {
                iPAddressText = value;
                OnPropertyChanged("IPAddressText");
            }
        }

        private string iPPortText;
        public string IPPortText
        {
            get { return iPPortText; }
            set
            {
                iPPortText = value;
                OnPropertyChanged("IPPortText");
            }
        }

        private ObservableCollection<LoggerItem> loggerItems = new ObservableCollection<LoggerItem>();
        public ObservableCollection<LoggerItem> LoggerItems
        {
            get { return loggerItems; }

            set
            {
                loggerItems = value;
                OnPropertyChanged("LoggerItems");
            }
        }

        public ICommand SetIPCommand { get; }
        public MBConnectViewModel()
        {
            SetIPCommand = new RelayCommand(ShowMsgTest, () => true);
        }

        private void LoggerItemAdd(string log)
        {
            loggerItems.Add(new LoggerItem(log));
        }

        public void ShowMsgTest()
        {
            SetSlaveIPAddrAndPort();
        }

        public void SetSlaveIPAddrAndPort()
        {
            if (!int.TryParse(IPPortText, out int portInt))
            {
                LoggerItemAdd("Wrong IP port");
                return;
            }

            if (_mbtcpconn.SetSlaveIPAddr(IPAddressText))
            {
                if (_mbtcpconn.SetSlaveIPPort(portInt))
                {
                    LoggerItemAdd("IP address Set");
                    return;
                }
                else
                {
                    LoggerItemAdd("Wrong IP port");
                    return;
                }
            }
            else
            {
                LoggerItemAdd("Wrong IP address format");
                return;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
