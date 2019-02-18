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

        public ObservableCollection<LoggerItem> loggerItems = new ObservableCollection<LoggerItem>();

        private void LoggerItemAdd(string log)
        {
            loggerItems.Add(new LoggerItem(log));
        }

        public void SetSlaveIPAddrAndPort(string ipAddr, string port)
        {
            if (!int.TryParse(port, out int portInt))
            {
                LoggerItemAdd("Wrong IP port");
            }

            if (_mbtcpconn.SetSlaveIPAddr(ipAddr))
            {
                if (_mbtcpconn.SetSlaveIPPort(portInt))
                {
                    LoggerItemAdd("IP address Set");
                }
                else
                {
                    LoggerItemAdd("Wrong IP port");
                }
            }
            else
            {
                LoggerItemAdd("Wrong IP address format");
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
