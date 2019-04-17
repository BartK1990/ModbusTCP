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
        private TcpClient _client;
        private IPAddress _iPSlaveAddr;
        private List<ModbusMsg> _readHoldingRegistersList = new List<ModbusMsg>();
        private string _ipSlaveAddrText;
        public string IpSlaveAddrText
        {
            get { return _iPSlaveAddr.ToString(); }
            private set
                { this.SetAndNotify(ref this._ipSlaveAddrText, value, () => this.IpSlaveAddrText); }
        }
        private int ipSlavePort;
        public int IPSlavePort
        {
            get { return ipSlavePort; }
            private set
                { this.SetAndNotify(ref this.ipSlavePort, value, () => this.IPSlavePort); }
        }
        private bool _ipAddrSet = false;
        private bool _ipPortSet = false;
        public bool _ipSet { get { return _ipAddrSet && _ipPortSet; } }
        readonly ILog _logger;

        public MBTCPConn()
        {
            IPSlavePort = -1;
        }
        public MBTCPConn(ILog logger)
        {
            IPSlavePort = -1;
            this._logger = logger;
        }

        public void CopyParametersAndInit(MBTCPConn mbTCPConn)
        {
            SetSlaveIPv4Addr(mbTCPConn.IpSlaveAddrText);
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
            info.AddValue("ipAddressText", IpSlaveAddrText);
            info.AddValue("ipPort", IPSlavePort);
        }

        private void Log(string message)
        {
            if (_logger != null)
                _logger.Log(message);
        }

        public int SetSlaveIPv4Addr(string ipAddr)
        {
            if (IPAddress.TryParse(ipAddr, out IPAddress ip))
            {
                _iPSlaveAddr = ip;
                IpSlaveAddrText = _iPSlaveAddr.ToString();
                Log("Ip Address Set");
                _ipAddrSet = true;
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
                _ipPortSet = true;
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
                if ((_iPSlaveAddr != null) && (IPSlavePort > -1))
                {
                    _client = new TcpClient();
                    await _client.ConnectAsync(_iPSlaveAddr, IPSlavePort);
                    Log("Connected to IP:" + _iPSlaveAddr.ToString() + " at port: " + IPSlavePort);
                    return 0;
                }
                else
                {
                    Log("Connection error at IP:" + _iPSlaveAddr.ToString() + " at port: " + IPSlavePort);
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
                if (_client != null)
                {
                    if (_client.Connected)
                    {
                        _client.Close();
                        Log("Disconnected successfully");
                        return 0;
                    }
                    Log("Disconnecting fault. There was no connection");
                    return 1;
                }
                else
                {
                    Log("Connection _client not created");
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
            var sb = new StringBuilder();

            Byte[] byteArray = mbtcpm.ReadHoldingRegisterSend(mm.Address, mm.Quantity);

            string hex = BitConverter.ToString(byteArray);
            monitor.Add(hex);
        }

        public void SendData(Byte[] byteArray)
        {
            // Translate the passed message into ASCII and store it as a Byte array.
            Byte[] data = byteArray;

            // Get a _client stream for reading and writing.
            //  Stream stream = _client.GetStream();

            NetworkStream stream = _client.GetStream();

            // Send the message to the connected TcpServer. 
            stream.Write(data, 0, data.Length);
            string hex = BitConverter.ToString(data);
            Console.Write("Sent: {0}", hex);

            // Receive the TcpServer.response.

            // Buffer to store the response bytes.
            data = new Byte[256];

            // Read the first batch of the TcpServer response bytes.
            stream.ReadTimeout = 5000;
            Int32 bytes = stream.Read(data, 0, data.Length);
            Byte[] dataReceived = new Byte[bytes];
            Array.Copy(data, dataReceived, bytes);

            hex = BitConverter.ToString(dataReceived);

            Console.WriteLine("Received: {0}", hex);

            // Close everything.
            stream.Close();
        }

    }
}
