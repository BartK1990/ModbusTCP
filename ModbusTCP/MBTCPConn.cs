using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusTCP
{
    using System.Net;
    using System.Net.Sockets;

    public class MBTCPConn
    {
        private TcpClient client;
        public IPAddress IPSlaveAddr { get; private set; }


        public void SetSlaveIPAddr(string IPAddr)
        {
            if(IPAddress.TryParse(IPAddr, out IPAddress ip))
            {
                IPSlaveAddr = ip;
                
            }

            
        }

        public void Connect()
        {
            client = new TcpClient();
            client.Connect(
        }
        
    }
}
