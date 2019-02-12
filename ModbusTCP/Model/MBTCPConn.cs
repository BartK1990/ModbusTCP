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

        public bool SetSlaveIPAddr(string ipAddr) // true if success and false if fault
        {
            if(IPAddress.TryParse(ipAddr, out IPAddress ip))
            {
                IPSlaveAddr = ip;
                return true;
            }
            else
                return false;
        }

        public bool SetSlaveIPPort(int port) // true if success and false if fault
        {
            if ((port >= 0) && (port <= 65535))
            {
                IPSlavePort = port;
                return true;
            }
            else
                return false;
        }

        public async Task<int> ConnectAsync()
        {
            try
            {
                if( (IPSlaveAddr != null) && (IPSlavePort > -1))
                {
                    client = new TcpClient();
                    await client.ConnectAsync(IPSlaveAddr, IPSlavePort);
                    return 0; // Connected succsefully
                }
                else
                {
                    return 1; // Wrong IP adres or Port
                }
            }
            catch
            {
                throw;
            }
        }

        public int Disconnect()
        {
            try
            {
                if (client.Connected)
                {
                    client.Close();
                    return 0; // Disconnected succsefully
                }
                return 1; // Client was not connected
            }
            catch
            {
                throw;
            }
        }
    }
}
