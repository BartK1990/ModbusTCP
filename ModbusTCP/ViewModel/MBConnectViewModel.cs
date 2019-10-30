﻿namespace ModbusTCP.ViewModel
{
    using Microsoft.Win32;
    using Model;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;

    public class MBConnectViewModel : ObservableObject
    {
        private MBTCPConn _mbtcpconn;
        public ObservableCollection<LoggerListBoxItem> LoggerItems { get; set; } = new ObservableCollection<LoggerListBoxItem>();

        public MBConnectViewModel(MBTCPConn mbTCPConn, WindowLogger windowLogger)
        {
            this._mbtcpconn = mbTCPConn;
            windowLogger.LoggerUpdated += LoggerUpdatedEventHandler;
            this._mbtcpconn.PropertyChanged += MBTCPConnEventHandler;
        }

        private string ipAddressSetText; // IP address that is set in MBTCP object
        public string IPAddressSetText
        {
            get { return this.ipAddressSetText; }
            set { this.SetAndNotify(ref this.ipAddressSetText, value, () => this.IPAddressSetText); }
        }

        private string ipAddressText;
        public string IPAddressText
        {
            get { return this.ipAddressText; }
            set { this.SetAndNotify(ref this.ipAddressText, value, () => this.IPAddressText); }
        }

        private string ipPortSetText;
        public string IPPortSetText
        {
            get { return this.ipPortSetText; }
            set { this.SetAndNotify(ref this.ipPortSetText, value, () => this.IPPortSetText); }
        }

        private string ipPortText = "502";
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
                   }, x => _mbtcpconn.IpSet));
            }
        }

        private ICommand _disconnectCommand;
        public ICommand DisconnectCommand
        {
            get
            {
                return _disconnectCommand ?? (_disconnectCommand = new RelayCommand(
                   x =>
                   {
                       Disconnect();
                   }, x => true));
            }
        }

        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new RelayCommand(
                   x =>
                   {
                       SaveMBTCPConn();
                   }, x => true));
            }
        }

        private ICommand _openCommand;
        public ICommand OpenCommand
        {
            get
            {
                return _openCommand ?? (_openCommand = new RelayCommand(
                   x =>
                   {
                       OpenMBTCPConn();
                   }, x => true));
            }
        }

        public void SetSlaveIPAddrAndPort()
        {
            _mbtcpconn.SetSlaveIPv4Address(IPAddressText);
            if (int.TryParse(IPPortText, out int portInt))
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

        public void SaveMBTCPConn()
        {
            Serializer serializer = new Serializer();
            SaveFileDialog dialog = new SaveFileDialog();
            if (dialog.ShowDialog() == true)
            {
                serializer.Binary_Serialize<MBTCPConn>(_mbtcpconn, dialog.FileName);
            }

        }

        public void OpenMBTCPConn()
        {
            Serializer serializer = new Serializer();
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                serializer.Binary_Deserialize<MBTCPConn>(out MBTCPConn mbtcpconn, dialog.FileName);
                _mbtcpconn.CopyParametersAndInit(mbtcpconn);
            }
        }

        private void LoggerUpdatedEventHandler(object sender, LoggerEventArgs e)
        {
            LoggerItemAdd(e.Log, e.Time.ToString("yyyy-MM-dd h:mm:ss"));
        }

        private void MBTCPConnEventHandler(object sender, PropertyChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.PropertyName))
            {
                if(e.PropertyName == "IPSlaveAddressText")
                {
                    IPAddressSetText = _mbtcpconn.IPSlaveAddressText;
                }
                if (e.PropertyName == "IPSlavePort")
                {
                    IPPortSetText = _mbtcpconn.IPSlavePort.ToString();
                }
            }
        }

        private void LoggerItemAdd(string log, string time)
        {
            LoggerItems.Add(new LoggerListBoxItem(log, time));
        }
    }
}
