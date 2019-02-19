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

    public class RelayCommand : ICommand
    {
        private Action _action;
        private Func<bool> canExecute;
        public RelayCommand(Action action, Func<bool> canExecute)
        {
            this._action = action;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            return canExecute();
        }

        public void Execute(object parameter)
        {
            _action();
        }
    }

    public class MBConnectViewModel : INotifyPropertyChanged
    {
        private MBTCPConn _mbtcpconn = new MBTCPConn();

        private string iPText;
        public string IPText
        {
            get { return iPText; }
            /*
            set
            {
                iPText = value;
                OnPropertyChanged("IPText");
            }
            */
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
            LoggerItemAdd("TEST");
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
