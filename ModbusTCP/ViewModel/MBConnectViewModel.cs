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

    using ModbusTCP;

    public struct LoggerListBoxItem
    {
        public string Log { get; set; }
        public string Time { get; set; }
        public LoggerListBoxItem(string log)
        {
            Log = log;
            Time = DateTime.Now.ToString();
        }
    }

    public class MBConnectViewModel : ObservableObject
    {
        private WindowLogger logger;
        private MBTCPConn _mbtcpconn;

        public MBConnectViewModel()
        {
            logger = App.WinLogger;
            logger.LoggerUpdated += LoggerUpdatedEventHandler;
            _mbtcpconn = new MBTCPConn(logger);
        }
  
        public ObservableCollection<LoggerListBoxItem> LoggerItems { get; set; } = new ObservableCollection<LoggerListBoxItem>();

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
                return;
            }

            if (_mbtcpconn.SetSlaveIPAddr(IPAddressText))
            {
                if (_mbtcpconn.SetSlaveIPPort(portInt))
                {
                    return;
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }

        private void LoggerUpdatedEventHandler(object sender, LoggerEventArgs e)
        {
            LoggerItemAdd(e.Log);
        }

        private void LoggerItemAdd(string log)
        {
            LoggerItems.Add(new LoggerListBoxItem(log));
        }
    }
}
