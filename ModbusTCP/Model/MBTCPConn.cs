using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModbusTCP.Model
{
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.Serialization;

    internal struct ModbusAddrQty
    {
        public int Address { get; set; }
        public int Quantity { get; set; }
        public ModbusAddrQty(int address, int quantity)
        {
            this.Address = address;
            this.Quantity = quantity;
        }
    }

    public struct ModbusMsg
    {
        public string Message { get; set; }
        public ModbusMsgType Type { get; set; }

        public ModbusMsg(string message, ModbusMsgType type)
        {
            Message = message;
            Type = type;
        }
    }

    public enum ModbusMsgType
    {
        Query = 1,
        Response = 2
    }

    [Serializable]
    public class MBTCPConn : ObservableObject, ISerializable
    {
        private TcpClient _client;
        private IPAddress _ipSlaveAddr;
        private Int32 _modbusDelay = 1000;
        private List<ModbusAddrQty> readHoldingRegistersList = new List<ModbusAddrQty>();
        public bool CommunicationPaused { get; private set; } = false;
        private string _ipSlaveAddressText;
        public string IPSlaveAddressText
        {
            get { return _ipSlaveAddr.ToString(); }
            private set
                { this.SetAndNotify(ref this._ipSlaveAddressText, value, () => this.IPSlaveAddressText); }
        }
        private int _ipSlavePort;
        public int IPSlavePort
        {
            get { return _ipSlavePort; }
            private set
                { this.SetAndNotify(ref this._ipSlavePort, value, () => this.IPSlavePort); }
        }
        private bool _ipAddrSet = false;
        private bool _ipPortSet = false;
        public bool IpSet { get { return _ipAddrSet && _ipPortSet; } }
        public bool Connected
        {
            get
            {
                if (this._client != null)
                    return _client.Connected;
                else
                    return false;
            }
        }
        private readonly ILog _logger;

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
            SetSlaveIPv4Address(mbTCPConn.IPSlaveAddressText);
            SetSlaveIPPort(mbTCPConn.IPSlavePort);
        }

        //Deserialization constructor.
        public MBTCPConn(SerializationInfo info, StreamingContext context)
        {
            SetSlaveIPv4Address((string)info.GetValue("ipAddressText", typeof(string)));
            SetSlaveIPPort((int)info.GetValue("ipPort", typeof(int)));
        }

        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ipAddressText", IPSlaveAddressText);
            info.AddValue("ipPort", IPSlavePort);
        }

        private void Log(string message)
        {
            if (_logger != null)
                _logger.Log(message);
        }

        public int SetSlaveIPv4Address(string ipAddr)
        {
            if (IPAddress.TryParse(ipAddr, out IPAddress ip))
            {
                _ipSlaveAddr = ip;
                IPSlaveAddressText = _ipSlaveAddr.ToString();
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
                if ((_ipSlaveAddr != null) && (IPSlavePort > -1))
                {
                    _client = new TcpClient();
                    await _client.ConnectAsync(_ipSlaveAddr, IPSlavePort);
                    Log("Connected to IP:" + _ipSlaveAddr.ToString() + " at port: " + IPSlavePort);
                    return 0;
                }
                else
                {
                    Log("Connection error at IP:" + _ipSlaveAddr.ToString() + " at port: " + IPSlavePort);
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
                    Log("Connection client not created");
                    return 2;
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task StartCommunication(IList<ModbusMsg> monitor)
        {
            CommunicationPaused = false;
            ModbusAddrQty mm = new ModbusAddrQty(1, 1);
            MBTCPMessages mbtcpm = new MBTCPMessages();
            NetworkStream stream = _client.GetStream();
            string timeFormat = "HH:mm:ss| ";

            while (_client.Connected && _client != null && !CommunicationPaused)
            {
                byte[] messageByteArray = mbtcpm.ReadHoldingRegisterSend(mm.Address, mm.Quantity);

                monitor.Add(new ModbusMsg(DateTime.Now.ToString(timeFormat) + BitConverter.ToString(messageByteArray), ModbusMsgType.Query));
                byte[] responseByteArray = await SendDataAsync(messageByteArray, stream);
                monitor.Add(new ModbusMsg(DateTime.Now.ToString(timeFormat) + BitConverter.ToString(responseByteArray), ModbusMsgType.Response));

                await Task.Delay(_modbusDelay);
            }

            // Close everything.
            stream.Close();
        }

        public async Task StopCommunication()
        {
            CommunicationPaused = true;
        }

        public async Task<byte[]> SendDataAsync(byte[] byteArray, NetworkStream ns)
        {
            // Translate the passed message into ASCII and store it as a Byte array.
            byte[] data = byteArray;

            // Send the message to the connected TcpServer. 
            await ns.WriteAsync(data, 0, data.Length);
            string hex = BitConverter.ToString(data);

            // Receive the TcpServer.response.
            // Buffer to store the response bytes.
            data = new byte[256];

            // Read the first batch of the TcpServer response bytes.
            ns.ReadTimeout = 3000;
            Int32 bytes = await ns.ReadAsync(data, 0, data.Length);
            byte[] dataReceived = new byte[bytes];
            Array.Copy(data, dataReceived, bytes);

            return dataReceived;
        }

    }
}
