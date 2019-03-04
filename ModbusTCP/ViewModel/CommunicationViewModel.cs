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
        private MBTCPConn _mbtcpconn;
        public ObservableCollection<CommunicationListBoxItem> CommunicationItems { get; set; } = new ObservableCollection<CommunicationListBoxItem>();

        public CommunicationViewModel(MBTCPConn mbTCPConn)
        {
            this._mbtcpconn = mbTCPConn;
        }

        private ICommand _sendCommand;
        public ICommand SendCommand
        {
            get
            {
                return _sendCommand ?? (_sendCommand = new RelayCommand(
                   x =>
                   {
                       _mbtcpconn.SendData(new Byte[] { 0x00, 0x01, 0x00, 0x00, 0x00, 0x05, 0x01, 0x03, 0x02, 0x00, 0x00 });
                   }, x => true));
            }
        }
    }
}
