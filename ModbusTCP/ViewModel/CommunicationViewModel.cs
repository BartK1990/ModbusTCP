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


    public class CommunicationViewModel : ObservableObject
    {
        private readonly MBTCPConn _mbtcpconn;
        public ObservableCollection<ModbusMsg> CommunicationItems { get; set; } = new ObservableCollection<ModbusMsg>();

        // ReSharper disable once InconsistentNaming - TCP is a shortcut
        public CommunicationViewModel(MBTCPConn mbTCPConn)
        {
            this._mbtcpconn = mbTCPConn;
        }

        private ICommand _startCommand;
        public ICommand StartCommand
        {
            get
            {
                return _startCommand ?? (_startCommand = new RelayCommand(
                   x =>
                   {
                       _mbtcpconn.StartCommunication(CommunicationItems);
                   }, x => _mbtcpconn.Connected));
            }
        }
        private ICommand _pauseCommand;
        public ICommand PauseCommand
        {
            get
            {
                return _pauseCommand ?? (_pauseCommand = new RelayCommand(
                           x =>
                           {
                               _mbtcpconn.StopCommunication();
                           }, x => _mbtcpconn.Connected));
            }
        }
    }
}
