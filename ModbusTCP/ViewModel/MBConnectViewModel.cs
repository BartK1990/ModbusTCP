using System.Net;
using System.Windows.Input;

namespace ModbusTCP.ViewModel
{

    public partial class CommunicationViewModel : ObservableObject
    {
        private ICommand _startCommand;
        public ICommand StartCommand
        {
            get
            {
                return _startCommand ?? (_startCommand = new RelayCommand(
                           x =>
                           {
                               _mbtcpconn.StartCommunication(CommunicationItems);
                           }, x => (_mbtcpconn.Connected && !_mbtcpconn.CommunicationInProgress)));
            }
        }
    }
}