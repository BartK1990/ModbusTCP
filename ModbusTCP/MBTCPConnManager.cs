using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusTCP
{
    public class MBTCPConnManager
    {
        private MBTCPConn mbtcpConn;

        public bool SetSlaveIPAddrAndPort(string ipAddr, string port, out string returnInfo)
        {
            mbtcpConn = new MBTCPConn();
            if (!int.TryParse(port, out int portInt))
            {
                returnInfo = "Wrong IP port";
                return false;
            }

            if (mbtcpConn.SetSlaveIPAddr(ipAddr))
            {             
                if (mbtcpConn.SetSlaveIPPort(portInt))
                {
                    returnInfo = "IP address Set";
                    return true;
                }
                else
                {
                    returnInfo = "Wrong IP port";
                    return false;
                }
            }
            else
            {
                returnInfo = "Wrong IP address format";
                return false;
            }
        }

        public async Task<string> ConnectAsync()
        {
            try
            {
                await mbtcpConn.ConnectAsync();
                return "Connected";
            }
            catch(Exception e)
            {
                return e.ToString();
            }
        }
    }
}
