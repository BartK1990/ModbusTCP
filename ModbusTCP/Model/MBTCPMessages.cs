using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusTCP.Model
{


    public static class MBTCPMessages
    {




        public static Byte[] ReadHoldingRegisterSend(int startAddress, int quantity)
        {
            Byte[] message = new Byte[1];

            return message;
        }
        public static List<int> ReadHoldingRegisterReceive(Byte[] response)
        {
            List<int> registersValues = new List<int>();

            return registersValues;
        }
    }
}
