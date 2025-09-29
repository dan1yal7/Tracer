using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WolfAnalyzer.Exceptions;
using WolfAnalyzer.NetworkInterfaceAnalyzer;
using WolfAnalyzer.TcpConnections;

namespace WolfAnalyzer.Logging
{
    public class LogWriter
    {
        public async void WriteLog(string message)
        {
            try
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                     "WolfAnalyzer",
                     "WolfAnalyzer.log");

                Directory.CreateDirectory(Path.GetDirectoryName(path) ?? throw new InvalidOperationException());
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                   await writer.WriteLineAsync($"{DateTime.Now} : {message}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"LogWriter did not work correctly: {ex.Message}");
            }
        }
    }
}
