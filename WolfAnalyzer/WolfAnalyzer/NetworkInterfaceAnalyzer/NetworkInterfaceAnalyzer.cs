using System.Diagnostics.Contracts;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using WolfAnalyzer.Exceptions;
using WolfAnalyzer.Logging;

namespace WolfAnalyzer.NetworkInterfaceAnalyzer
{
    public class NetworkInterfaceAnalyzer
    {
        public void GetInterfaceTraffic()
        {
            try
            {
                var adapters = NetworkInterface.GetAllNetworkInterfaces();
                Console.WriteLine($"Amount: {adapters.Length}");
                foreach (var adapter in adapters)
                {
                    Console.WriteLine("=====================================================================");
                    Console.WriteLine($"Device Id: {adapter.Id}");
                    Console.WriteLine($"Name: {adapter.Name}");
                    Console.WriteLine($"NetworkInterfaceType: {adapter.NetworkInterfaceType}");
                    Console.WriteLine($"Status: {adapter.OperationalStatus}");
                    Console.WriteLine($"Speed: {adapter.Speed}");

                    IPInterfaceStatistics stat = adapter.GetIPStatistics();
                    Console.WriteLine($"Recieved: {stat.BytesReceived}");
                    Console.WriteLine($"Sent: {stat.BytesSent}");


                    var logMessage = ""
                        + $"\nDevice Id: {adapter.Id}"
                        + $"\nName: {adapter.Name}"
                        + $"\nNetworkInterfaceType: {adapter.NetworkInterfaceType}"
                        + $"\nStatus: {adapter.OperationalStatus}"
                        + $"\nSpeed: {adapter.Speed}"
                        + $"\nRecieved: {stat.BytesReceived}"
                        + $"\nSent: {stat.BytesSent}"
                        + $"\n----------------------------------------------\n";

                    LogWriter logWriter = new LogWriter();
                    logWriter.WriteLog(logMessage);
                }
            }
            catch (NetworkIntefaceAnalyzerException ex)
            {
                throw new NetworkIntefaceAnalyzerException($"NetworkInterfaceAnalyzer did not work correctly: {ex.Message}");
            }
        }
    }
}
