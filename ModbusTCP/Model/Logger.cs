using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusTCP.Model
{

    public class LoggerEventArgs : EventArgs
    {
        public string Log { get; private set; }
        public DateTime Time { get; private set; }
        public LoggerEventArgs(string log)
        {
            Time = DateTime.Now;
            Log = log;
        }
    }

    public interface ILog
    {
        void Log(string message);
    }

    public class FileLogger : ILog
    {
        private static readonly object locker;
        private string filePath = Directory.GetCurrentDirectory() + @"IDGLog.txt";
        public void Log(string message)
        {
            lock (locker)
            {
                using (StreamWriter streamWriter = new StreamWriter(filePath))
                {
                    streamWriter.WriteLine(message);
                    streamWriter.Close();
                }
            }
        }
    }

    public class DatabaseLogger : ILog
    {
        public void Log(string message)
        {
            throw new NotImplementedException();
        }
    }

    public class WindowLogger : ILog
    {
        public void Log(string message)
        {
            OnLoggerUpdated(message);
        }

        public event EventHandler<LoggerEventArgs> LoggerUpdated;
        private void OnLoggerUpdated(string log)
        {
            EventHandler<LoggerEventArgs> loggerUpdated = LoggerUpdated;
            if (loggerUpdated != null)
            {
                loggerUpdated(this, new LoggerEventArgs(log));
            }
        }
    }

}
