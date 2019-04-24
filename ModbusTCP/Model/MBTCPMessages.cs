using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusTCP.Model
{



    public class MBTCPMessages
    {
        private uint transactionIdentifierInternal = 0;
        private byte[] transactionIdentifier = new byte[2];
        private byte[] protocolId = new byte[2];
        private byte[] crc = new byte[2];
        private byte[] length = new byte[2];
        private byte unitIdentifier = 0x01;
        private byte functionCode;
        private byte[] startingAddress = new byte[2];
        private byte[] quantity = new byte[2];

        public byte[] ReadHoldingRegisterSend(int startingAddress, int quantity)
        {
            transactionIdentifierInternal++;

            this.transactionIdentifier = BitConverter.GetBytes((uint)transactionIdentifierInternal);
            this.protocolId = BitConverter.GetBytes((int)0x0000);
            this.length = BitConverter.GetBytes((int)0x0006);
            this.functionCode = 0x03;
            this.startingAddress = BitConverter.GetBytes(startingAddress);
            this.quantity = BitConverter.GetBytes(quantity);
            byte[] message = new byte[]{   this.transactionIdentifier[1],
                            this.transactionIdentifier[0],
                            this.protocolId[1],
                            this.protocolId[0],
                            this.length[1],
                            this.length[0],
                            this.unitIdentifier,
                            this.functionCode,
                            this.startingAddress[1],
                            this.startingAddress[0],
                            this.quantity[1],
                            this.quantity[0],
                            this.crc[0],
                            this.crc[1]
            };

            return message;
        }
        public List<int> ReadHoldingRegisterReceive(Byte[] response)
        {
            List<int> registersValues = new List<int>();

            return registersValues;
        }
    }
}
