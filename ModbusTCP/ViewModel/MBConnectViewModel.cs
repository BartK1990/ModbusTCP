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
    using Microsoft.Win32;
    using ModbusTCP;
    using System.Runtime.Serialization;


    public class MBConnectViewModel : ObservableObject, ISerializable
    {
        public MBConnectViewModel()
        {
            App.WinLogger.LoggerUpdated += LoggerUpdatedEventHandler;
            _mbtcpconn = new MBTCPConn(App.WinLogger);
        }

        //Deserialization constructor.
        public MBConnectViewModel(SerializationInfo info, StreamingContext context)
        {
            ipAddressText = (string)info.GetValue("ipAddressText", typeof(string));
            ipPortText = (string)info.GetValue("ipPortText", typeof(string));
        }

        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ipAddressText", ipAddressText);
            info.AddValue("ipPortText", ipPortText);
        }

        private MBTCPConn _mbtcpconn;
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
                   }, x => _mbtcpconn.ipSet));
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
            _mbtcpconn.SetSlaveIPAddr(IPAddressText);
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
            throw new NotImplementedException();
            /*
            Serializer<MBConnectViewModel> serializer = new Serializer<MBConnectViewModel>();
            SaveFileDialog dialog = new SaveFileDialog();
            if (dialog.ShowDialog() == true)
            {
                serializer.Binary_Serialize(this, dialog.FileName);
            }
            */
        }

        public void OpenMBTCPConn()
        {
            throw new NotImplementedException();
            /*
            Serializer<MBConnectViewModel> serializer = new Serializer<MBConnectViewModel>();
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                serializer.Binary_Deserialize(out this, dialog.FileName);
            }
            */
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
