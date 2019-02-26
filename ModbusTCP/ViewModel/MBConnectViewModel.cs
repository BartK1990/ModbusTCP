﻿using System;
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



    public class MBConnectViewModel : ObservableObject
    {
        private MBTCPConn _mbtcpconn;

        public MBConnectViewModel()
        {
            App.WinLogger.LoggerUpdated += LoggerUpdatedEventHandler;
            _mbtcpconn = new MBTCPConn(App.WinLogger);
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

        private ICommand _connectCommand;
        public ICommand ConnectCommand
        {
            get
            {
                return _connectCommand ?? (_connectCommand = new RelayCommand(
                   x =>
                   {
                       Connect();
                   }, x => true));
            }
        }

        public void SetSlaveIPAddrAndPort()
        {
            _mbtcpconn.SetSlaveIPAddr(IPAddressText);
            if (!int.TryParse(IPPortText, out int portInt))
            {
                _mbtcpconn.SetSlaveIPPort(portInt);
            }
        }

        public void Connect()
        {
            _mbtcpconn.ConnectAsync();
        }

        public void Disconnect()
        {
            _mbtcpconn.Disconnect();
        }

        private void LoggerUpdatedEventHandler(object sender, LoggerEventArgs e)
        {
            LoggerItemAdd(e.Log, e.Time.ToString("yyyy-MM-dd h:mm:ss"));
        }

        private void LoggerItemAdd(string log, string time)
        {
            LoggerItems.Add(new LoggerListBoxItem(log, time));
        }
    }
}
