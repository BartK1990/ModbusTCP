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
        public ObservableCollection<string> CommunicationItems { get; set; } = new ObservableCollection<string>();

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
                       _mbtcpconn.StartCommunnication(CommunicationItems);
                   }, x => true));
            }
        }
    }
}
