﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
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
        private int _modbusDelay = 1000; // in milliseconds
        private short _connectingTimeout = 5000; // in milliseconds
        private string messageTimeFormat = "HH:mm:ss| ";
        private List<ModbusAddrQty> readHoldingRegistersList = new List<ModbusAddrQty>();
        private IPAddress _ipSlaveAddress;
        private string _ipSlaveAddressText;
        public string IPSlaveAddressText
        {
            get => _ipSlaveAddress.ToString();
            private set
                { this.SetAndNotify(ref this._ipSlaveAddressText, value, () => this.IPSlaveAddressText); }
        }
        private int _ipSlavePort;
        public int IPSlavePort
        {
            get => _ipSlavePort;
            private set
                { this.SetAndNotify(ref this._ipSlavePort, value, () => this.IPSlavePort); }
        }
        private bool _ipAddressSet = false;
        private bool _ipPortSet = false;
        public bool IpSet => _ipAddressSet && _ipPortSet;
        private bool communicating;
        public bool Communicating
        {
            get => communicating;
            private set { this.SetAndNotify(ref this.communicating, value, () => this.Communicating); }
        }
        private TcpState tcpState;
        public TcpState TcpState 
        {
            get => tcpState;
            private set { this.SetAndNotify(ref this.tcpState, value, () => this.TcpState); }
        }
        private bool connected;
        public bool Connected
        {
            get => connected;
            private set { this.SetAndNotify(ref this.connected, value, () => this.Connected); }
        }
        public string MessageSent { get; private set; }
        public string MessageReceived { get; private set; }
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
        public MBTCPConn(SerializationInfo info, StreamingContext context) //Deserialization constructor
        {
            SetSlaveIPv4Address((string)info.GetValue("ipAddressText", typeof(string)));
            SetSlaveIPPort((int)info.GetValue("ipPort", typeof(int)));
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context) //Serialization function
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
                _ipSlaveAddress = ip;
                IPSlaveAddressText = _ipSlaveAddress.ToString();
                Log("Ip Address Set");
                _ipAddressSet = true;
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
        private void UpdateConnectionState()
        {
            if (_client != null)
            {
                Connected = _client.Client != null && _client.Connected && SocketConnected();
                var state =_client.GetState();
                if(TcpState != state)
                {
                    TcpState = state;
                    Log($"Tcp state changed. Current state: {state}");
                }
            }
            else
            {
                Connected = false;
            }
            if (!Connected)
            {
                Log("Tcp client is not connected");
                Communicating = false;
            }
        }

        private bool SocketConnected()
        {
            if (_client?.Client == null) return false;
            var s = _client.Client;
            return !s.Poll(0, SelectMode.SelectRead) || (s.Available != 0);
        }

        public async void ConnectAsync()
        {
            try
            {
                // This check should be done in some better way
                if ((_ipSlaveAddress != null) && (IPSlavePort > -1))
                {
                    _client = new TcpClient();
                    // Async connection limited by configurable property in milliseconds
                    var executionInTime = await Task.Run(() =>
                        _client.ConnectAsync(_ipSlaveAddress, IPSlavePort).Wait(_connectingTimeout));
                    if (executionInTime)
                    {
                        Log("Connected. IP:" + _ipSlaveAddress.ToString() + " port: " + IPSlavePort);
                        UpdateConnectionState();
                        CheckConnection();
                        return;
                    }
                    else
                    {
                        Log("Connection timeout. IP:" + _ipSlaveAddress.ToString() + " port: " + IPSlavePort);
                        return;
                    }
                }
                else
                {
                    Log("Wrong connection parameters. IP:" + _ipSlaveAddress.ToString() + " port: " + IPSlavePort);
                }
            }
            catch
            {
                Log("Connection error at IP." + _ipSlaveAddress.ToString() + " port: " + IPSlavePort);
            }
            finally
            {
                UpdateConnectionState();
            }
        }

        private async Task CheckConnection() // periodically check TCP connection until communication is not started
        {
            if(_client == null)
                return;
            while (Connected && !Communicating)
            {
                UpdateConnectionState();
                await Task.Delay(_modbusDelay); // Delay to make pause between checks
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
            finally
            {
                UpdateConnectionState();
            }
        }
        public async Task StartCommunication(IList<ModbusMsg> monitor) // main async Task handling communication
        {
            ModbusAddrQty mm = new ModbusAddrQty(1, 1);
            MBTCPMessages mbtcpm = new MBTCPMessages();
            using (NetworkStream stream = _client.GetStream())
            {
                while (_client != null && _client.Connected)
                {
                    Communicating = true;
                    byte[] messageByteArray = mbtcpm.ReadHoldingRegisterSend(mm.Address, mm.Quantity);
                    monitor.Add(new ModbusMsg(
                        DateTime.Now.ToString(messageTimeFormat) + BitConverter.ToString(messageByteArray),
                        ModbusMsgType.Query));
                    MessageSent = BitConverter.ToString(messageByteArray);
                    byte[] responseByteArray = await SendDataAsync(messageByteArray, stream);
                    MessageReceived = BitConverter.ToString(responseByteArray);
                    monitor.Add(new ModbusMsg(
                        DateTime.Now.ToString(messageTimeFormat) + BitConverter.ToString(responseByteArray),
                        ModbusMsgType.Response));
                    await Task.Delay(_modbusDelay); // Delay to make pause between messages
                }
            }
            UpdateConnectionState();
        }
        private async Task<byte[]> SendDataAsync(byte[] byteArray, NetworkStream ns)
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
