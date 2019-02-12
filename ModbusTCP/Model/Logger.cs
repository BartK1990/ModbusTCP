using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusTCP
{

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

}
