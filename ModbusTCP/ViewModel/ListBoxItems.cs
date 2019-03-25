using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusTCP.ViewModel
{
    public struct LoggerListBoxItem
    {
        public string Log { get; set; }
        public string Time { get; set; }
        public LoggerListBoxItem(string log)
        {
            Log = log;
            Time = DateTime.Now.ToString();
        }
        public LoggerListBoxItem(string log, string time)
        {
            Log = log;
            Time = time;
        }
    }
}
