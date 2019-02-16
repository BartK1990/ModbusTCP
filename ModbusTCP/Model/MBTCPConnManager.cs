using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusTCP.Model
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

        public async void ConnectAsync()
        {
            /*
            if(mbtcpConn != null)
            {
                try
                {
                    switch (await mbtcpConn.ConnectAsync())
                    {
                        case 0:
                            LoggerManager.LogToMainWindow("Connected to IP:" + mbtcpConn.IPSlaveAddr.ToString() + " at port: " + mbtcpConn.IPSlavePort);
                            break;
                        case 1:
                            LoggerManager.LogToMainWindow("Wrong connection settings. IP: " + mbtcpConn.IPSlaveAddr.ToString() + " port: " + mbtcpConn.IPSlavePort);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    LoggerManager.LogToMainWindow("Connection error to IP:" + mbtcpConn.IPSlaveAddr.ToString() + " at port: " + mbtcpConn.IPSlavePort);
                    LoggerManager.LogToMainWindow(e.ToString());
                }
            }
            else
                LoggerManager.LogToMainWindow("IP address and port not set!");
            */
        }

        public void Disconnect()
        {
            if(mbtcpConn != null)
            {
                // implementation
            }
            throw new NotImplementedException();
            
        }

    }
}
