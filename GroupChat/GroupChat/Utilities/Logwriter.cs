using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupChat.Utilities
{
    public static class Logwriter
    {
        static System.IO.StreamWriter _errorLog;
        static System.IO.StreamWriter _connectionLog;

        public static void InitialiseBuffers(string applicationPath)
        {
            string path = applicationPath + "\\Logs\\ErrorLogs " + DateTime.Now.ToString("dd-MMM-yy") + ".txt";
            _errorLog = new System.IO.StreamWriter(path);

            path = applicationPath + "\\Logs\\ConnectionLogs " + DateTime.Now.ToString("dd-MMM-yy") + ".txt";
            _connectionLog = new System.IO.StreamWriter(path);
        }

        public static void WriteToErrorLogs(string text)
        {
            _errorLog.WriteLine(text);
            _errorLog.Flush();

        }

        public static void WriteToConnectionLogs(string text)
        {
            _connectionLog.WriteLine(text);
            _connectionLog.Flush();
        }

    }
}
