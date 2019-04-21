using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusTCP.Model
{
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.Serialization;

    internal struct ModbusMsg
    {
        public int Address { get; set; }
        public int Quantity { get; set; }
        public ModbusMsg(int address, int quantity)
        {
            this.Address = address;
            this.Quantity = quantity;
        }
    }

    [Serializable]
    public class MBTCPConn : ObservableObject, ISerializable
    {
        private TcpClient client;
        private IPAddress iPSlaveAddr;
        private List<ModbusMsg> readHoldingRegistersList = new List<ModbusMsg>();
        private string ipSlaveAddrText;
        public string IPSlaveAddrText
        {
            get { return iPSlaveAddr.ToString(); }
            private set
                { this.SetAndNotify(ref this.ipSlaveAddrText, value, () => this.IPSlaveAddrText); }
        }
        private int ipSlavePort;
        public int IPSlavePort
        {
            get { return ipSlavePort; }
            private set
                { this.SetAndNotify(ref this.ipSlavePort, value, () => this.IPSlavePort); }
        }
        private bool ipAddrSet = false;
        private bool ipPortSet = false;
        public bool ipSet { get { return ipAddrSet && ipPortSet; } }
        private ILog logger;

        public MBTCPConn()
        {
            IPSlavePort = -1;
        }
        public MBTCPConn(ILog logger)
        {
            IPSlavePort = -1;
            this.logger = logger;
        }

        public void CopyParametersAndInit(MBTCPConn mbTCPConn)
        {
            SetSlaveIPv4Addr(mbTCPConn.IPSlaveAddrText);
            SetSlaveIPPort(mbTCPConn.IPSlavePort);
        }

        //Deserialization constructor.
        public MBTCPConn(SerializationInfo info, StreamingContext context)
        {
            SetSlaveIPv4Addr((string)info.GetValue("ipAddressText", typeof(string)));
            SetSlaveIPPort((int)info.GetValue("ipPort", typeof(int)));
        }

        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ipAddressText", IPSlaveAddrText);
            info.AddValue("ipPort", IPSlavePort);
        }

        private void Log(string message)
        {
            if (logger != null)
                logger.Log(message);
        }

        public int SetSlaveIPv4Addr(string ipAddr)
        {
            if (IPAddress.TryParse(ipAddr, out IPAddress ip))
            {
                iPSlaveAddr = ip;
                IPSlaveAddrText = iPSlaveAddr.ToString();
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
                if ((iPSlaveAddr != null) && (IPSlavePort > -1))
                {
                    client = new TcpClient();
                    await client.ConnectAsync(iPSlaveAddr, IPSlavePort);
                    Log("Connected to IP:" + iPSlaveAddr.ToString() + " at port: " + IPSlavePort);
                    return 0;
                }
                else
                {
                    Log("Connection error at IP:" + iPSlaveAddr.ToString() + " at port: " + IPSlavePort);
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
                        Log("Disconnected successfully");
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

        async public void StartCommunnication(IList<string> monitor)
        {
            ModbusMsg mm = new ModbusMsg(1, 1);
            MBTCPMessages mbtcpm = new MBTCPMessages();

            Byte[] messageByteArray = mbtcpm.ReadHoldingRegisterSend(mm.Address, mm.Quantity);
            Byte[] responseByteArray = await SendDataAsync(messageByteArray);
            string hex = BitConverter.ToString(messageByteArray);
            monitor.Add(hex);



        }

        public async Task<Byte[]> SendDataAsync(Byte[] byteArray)
        {
            // Translate the passed message into ASCII and store it as a Byte array.
            Byte[] data = byteArray;

            // Get a client stream for reading and writing.
            //  Stream stream = client.GetStream();

            NetworkStream stream = client.GetStream();

            // Send the message to the connected TcpServer. 
            await stream.WriteAsync(data, 0, data.Length);
            string hex = BitConverter.ToString(data);

            // Receive the TcpServer.response.

            // Buffer to store the response bytes.
            data = new Byte[256];

            // Read the first batch of the TcpServer response bytes.
            stream.ReadTimeout = 3000;
            Int32 bytes = await stream.ReadAsync(data, 0, data.Length);
            Byte[] dataReceived = new Byte[bytes];
            Array.Copy(data, dataReceived, bytes);

            // Close everything.
            stream.Close();

            return dataReceived;
        }

    }
}
