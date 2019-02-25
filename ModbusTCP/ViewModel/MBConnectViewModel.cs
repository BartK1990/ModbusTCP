using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusTCP.ViewModel
{
    using Model;
    using System.Collections.ObjectModel;
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

    public class MBConnectViewModel : ObservableObject
    {
        private MBTCPConn _mbtcpconn = new MBTCPConn();
        public ObservableCollection<LoggerItem> LoggerItems { get; set; } = new ObservableCollection<LoggerItem>();

        private string ipAddressText;
        public string IPAddressText
        {
            get { return this.ipAddressText; }
            set { this.SetAndNotify(ref this.ipAddressText, value, () => this.IPAddressText); }
        }

        private string ipPortText;
        public string IPPortText
        {
            get { return this.ipPortText; }
            set { this.SetAndNotify(ref this.ipPortText, value, () => this.IPPortText); }
        }

        private ICommand _setIPCommand;
        public ICommand SetIPCommand
        {
            get
            {
                return _setIPCommand ?? (_setIPCommand = new RelayCommand(
                   x =>
                   {
                        SetSlaveIPAddrAndPort();
                   }, x => true));
            }
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

        private void LoggerItemAdd(string log)
        {
            LoggerItems.Add(new LoggerItem(log));
        }
    }
}
