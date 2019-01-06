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
        public int IPSlavePort { get; private set; }

        public MBTCPConn()
        {
            IPSlavePort = -1;
        }

        public bool SetSlaveIPAddr(string IPAddr) // true if success and false if fault
        {
            if(IPAddress.TryParse(IPAddr, out IPAddress ip))
            {
                IPSlaveAddr = ip;
                return true;
            }
            else
                return false;
        }

        public async Task ConnectAsync()
        {
            try
            {
                if( (IPSlaveAddr != null) && (IPSlavePort > -1))
                {
                    using (client = new TcpClient())
                    {
                        await client.ConnectAsync(IPSlaveAddr, IPSlavePort);
                        using (NetworkStream ns = client.GetStream())
                        {

                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
