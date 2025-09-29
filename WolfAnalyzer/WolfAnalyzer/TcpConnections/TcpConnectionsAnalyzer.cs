using System.Net.NetworkInformation;
using WolfAnalyzer.Exceptions;
using WolfAnalyzer.Logging;

namespace WolfAnalyzer.TcpConnections
{
    public class TcpConnectionsAnalyzer
    {
        public void GetTcpConnectionTraffic()
        {
            try
            {
                var connections = IPGlobalProperties.GetIPGlobalProperties();
                var tcpConnections = connections.GetActiveTcpConnections();
                var udpConnections = connections.GetActiveUdpListeners();

                Console.WriteLine($"TCPconnections in total: {tcpConnections.Length}");
                foreach (var connection in tcpConnections)
                {
                    Console.WriteLine("=============================================");
                    Console.WriteLine($"Local endpoint: {connection.LocalEndPoint}");
                    Console.WriteLine($"Remote endpoint: {connection.RemoteEndPoint}");
                    Console.WriteLine($"State: {connection.State}");
                }

                Console.WriteLine($"UDPconnections in total: {udpConnections.Length}");

                var logMessage = $""
                    + $"\nTCPconnections in total: {tcpConnections.Length}"
                    + $"\nUDPconnections in total: {udpConnections.Length}"
                    + $"\n----------------------------------------------\n";

                LogWriter logWriter = new LogWriter();
                logWriter.WriteLog(logMessage);
            }
            catch (TcpConnectionAnalyzerException ex)
            {
                throw new TcpConnectionAnalyzerException($"TcpConnectionAnalyzer did not work correctly: {ex.Message}");
            }
        }
    }
}
