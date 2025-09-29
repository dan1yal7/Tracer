using System.Net.NetworkInformation;
using WolfAnalyzer.Exceptions;
using WolfAnalyzer.Logging;
using WolfAnalyzer.NetworkInterfaceAnalyzer;
using WolfAnalyzer.TcpConnections;
using WolfAnalyzer.Tracer;

try
{
    //NetworkInterfaces
    NetworkInterfaceAnalyzer networkInterfaceAnalyzer = new NetworkInterfaceAnalyzer();
    networkInterfaceAnalyzer.GetInterfaceTraffic();
    Console.WriteLine($"{networkInterfaceAnalyzer}");

    //tcp connections
    TcpConnectionsAnalyzer tcpConnectionsAnalyzer = new TcpConnectionsAnalyzer();
    tcpConnectionsAnalyzer.GetTcpConnectionTraffic();
    Console.WriteLine($"{tcpConnectionsAnalyzer}");

    //Traffic scanning 
    var trafficAnalyzer = IPGlobalProperties.GetIPGlobalProperties();
    var trafficStatics = trafficAnalyzer.GetIPv4GlobalStatistics();
    Console.WriteLine($"[{DateTime.Now}]");
    Console.WriteLine($"Incoming Packages: {trafficStatics.ReceivedPackets}");
    Console.WriteLine($"Outgoing Packages: {trafficStatics.OutputPacketRequests}");
    Console.WriteLine($"Discarded Incoming Packages: {trafficStatics.ReceivedPacketsDiscarded}");
    Console.WriteLine($"Discarded Outgoing Packages: {trafficStatics.OutputPacketRoutingDiscards}");
    Console.WriteLine($"Received Packets With Unknown Protocol: {trafficStatics.ReceivedPacketsWithUnknownProtocol}");

    var logMessage = $""
        + $"\nIncoming Packages: {trafficStatics.ReceivedPackets}"
        + $"\nOutgoing Packages: {trafficStatics.OutputPacketRequests}"
        + $"\nDiscarded Incoming Packages: {trafficStatics.ReceivedPacketsDiscarded}"
        + $"\nDiscarded Outgoing Packages: {trafficStatics.OutputPacketRoutingDiscards}"
        + $"\nReceived Packets With Unknown Protocol: {trafficStatics.ReceivedPacketsWithUnknownProtocol}"
        + $"\n----------------------------------------------\n";

    LogWriter logWriter = new LogWriter();
    logWriter.WriteLog(logMessage);

    WebTracer webTracer = new WebTracer();
    webTracer.TraceWebActivity();
}
catch (AnalyzerException ex)
{
    throw new AnalyzerException($"Analyzer did not work correctly: {ex.Message}");
}