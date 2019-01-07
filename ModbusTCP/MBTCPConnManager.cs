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

        public bool SetSlaveIPAddrAndPort(string ipAddr, int port, out string returnInfo)
        {
            mbtcpConn = new MBTCPConn();
            if (mbtcpConn.SetSlaveIPAddr(ipAddr))
            {             
                if (mbtcpConn.SetSlaveIPPort(port))
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
    }
}
