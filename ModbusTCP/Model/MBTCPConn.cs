using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusTCP.Model
{
    using System.Net;
    using System.Net.Sockets;

    public class MBTCPConn
    {
        private TcpClient client;
        public IPAddress IPSlaveAddr { get; private set; }
        public int IPSlavePort { get; private set; }
        private bool ipAddrSet = false;
        private bool ipPortSet = false;
        public bool ipSet { get { return ipAddrSet && ipPortSet; } }
        private ILog logger;

        public MBTCPConn(ILog logger)
        {
            IPSlavePort = -1;
            this.logger = logger;
        }

        public MBTCPConn()
        {
            IPSlavePort = -1;
        }

        private void Log(string message)
        {
            if (logger != null)
                logger.Log(message);
        }

        public int SetSlaveIPAddr(string ipAddr)
        {
            if (IPAddress.TryParse(ipAddr, out IPAddress ip))
            {
                IPSlaveAddr = ip;
                Log("Ip Address Set");
                ipAddrSet = true;
                return 0;
            }
            else
            {
                Log("Ip Address setting fault");
                return 1;
            }
        }

        public int SetSlaveIPPort(int port)
        {
            if ((port >= 0) && (port <= 65535))
            {
                IPSlavePort = port;
                Log("Ip Port set");
                ipPortSet = true;
                return 0;
            }
            else
            {
                Log("Ip Port setting fault");
                return 1;
            }
        }

        public async Task<int> ConnectAsync()
        {
            try
            {
                if ((IPSlaveAddr != null) && (IPSlavePort > -1))
                {
                    client = new TcpClient();
                    await client.ConnectAsync(IPSlaveAddr, IPSlavePort);
                    Log("Connected to IP:" + IPSlaveAddr.ToString() + " at port: " + IPSlavePort);
                    return 0;
                }
                else
                {
                    Log("Connection error at IP:" + IPSlaveAddr.ToString() + " at port: " + IPSlavePort);
                    return 1;
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
                if (client != null)
                {
                    if (client.Connected)
                    {
                        client.Close();
                        Log("Disconnected succesfully");
                        return 0;
                    }
                    Log("Disconnecting fault. There was no connection");
                    return 1;
                }
                else
                {
                    Log("Connection client not created");
                    return 2;
                }
            }
            catch
            {
                throw;
            }
        }

        public void SendData()
        {
            throw new NotImplementedException();
        }
    }
}
