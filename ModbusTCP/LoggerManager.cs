using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ModbusTCP
{
    public static class LoggerManager
    {
        static MainWindow mw = (MainWindow)Application.Current.MainWindow;

        struct LoggerItem
        {
            public string Log { get; set; }
            public string Time { get; set; }
            public LoggerItem(string log)
            {
                Log = log;
                Time = DateTime.Now.ToString();
            }
        }

        public static void LogMainWindow(string log)
        {
            mw.Logger.Items.Insert(0, new LoggerItem(log));
        }
    }
}
