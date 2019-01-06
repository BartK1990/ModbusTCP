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

        public bool SetSlaveIPAddr(string ipAddr)
        {
            mbtcpConn = new MBTCPConn();
            if (mbtcpConn.SetSlaveIPAddr(ipAddr))
                return true;
            else
                return false;
        }
    }
}
