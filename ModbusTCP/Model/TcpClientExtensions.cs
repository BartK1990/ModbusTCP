using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ModbusTCP.Model
{
    public static class TcpClientExtensions
    {
        public static TcpState GetState(this TcpClient tcpClient)
        {
            if (tcpClient.Client == null)
                return TcpState.Closed;
            try
            {
                var connectionInformation = IPGlobalProperties.GetIPGlobalProperties()
                    .GetActiveTcpConnections()
                    .FirstOrDefault(x => x.LocalEndPoint.Equals(tcpClient.Client.LocalEndPoint));
                return connectionInformation?.State ?? TcpState.Unknown;
            }
            catch
            {
                return TcpState.Closed;
            }
        }
    }
}
