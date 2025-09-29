using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Session;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WolfAnalyzer.Tracer
{
    public class WebTracer
    {
        public int TraceWebActivity()
        {
            // Get some data and put it in a file
            CollectData("Microsoft-Windows-DNS-Client", "Analytical.etl");

            ProcessData("Analytical.etl");
            return 0;
        }


        /// <summary>
        /// CollectData turn on logging of data from 'eventSourceName' to the file 'dataFileName'.
        /// It will then call EventGenerator.CreateEvents and wait 12 seconds for it to generate some data.  
        /// </summary>
        private void CollectData(string eventSourceName, string dataFileName)
        {
            if (!(TraceEventSession.IsElevated() ?? false))
            {   
                Console.WriteLine("To turn on ETW events you need to be administrator, please run from an Admin process");
                Debugger.Break();
                return;
            }

            var sessionName = "WebTracingEvent";
            Console.WriteLine($"Creating a {0} session writing to {1} {sessionName}, {dataFileName}");

            using (var session = new TraceEventSession(sessionName, dataFileName))
            {
                Console.CancelKeyPress += delegate (object? s, ConsoleCancelEventArgs e) { session.Dispose(); };
               
                var restarted = session.EnableProvider("Microsoft-Windows-DNS-Client");
                if (restarted)
                {
                    Console.WriteLine($"The session was restarted, some events might have been lost {sessionName}");
                }
                Console.WriteLine("Waiting 12 seconds for events to come in.");
                Thread.Sleep(12000);
            }
        }


        /// <summary>
        /// Process the data in 'dataFileName' printing the events and doing delta computation between 'MyFirstEvent'
        /// and 'MySecondEvent'.  
        /// </summary>

        private static void ProcessData(string dataFileName)
        {
            Console.WriteLine($"Opening to see what data is in the file {dataFileName}");
            using (var source = new ETWTraceEventSource(dataFileName))
            {
                if (source.EventsLost > 0)
                    Console.WriteLine($"Events were {0} lost during the session {source.EventsLost}");

                // To demonstrate non-trivial event manipulation, we calculate the time delta between 'MyFirstEvent and 'MySecondEvent'
                // firstEventTimeMSec remembers all the 'MyFirstEvent' arrival times (indexed by their ID)  
                var firstEventTimeMSec = new Dictionary<int, double>();


                /*****************************************************************************************************/
                // Hook up events.   To so this first we need a 'Parser. which knows how to part the events of a particular Event Provider.
                // In this case we get a DynamicTraceEventSource, which knows how to parse any EventSource provider.    This parser
                // is so common, that TraceEventSource as a shortcut property called 'Dynamic' that fetches this parsers.  

                // For debugging, and demo purposes, hook up a callback for every event that 'Dynamic' knows about (this is not EVERY
                // event only those know about by DynamiceTraceEventParser).   However the 'UnhandledEvents' handler below will catch
                // the other ones.
                source.Dynamic.All += delegate (TraceEvent data)
                {
                    Console.WriteLine(data.PayloadByName("MyName"));
                    Console.WriteLine("Got Event:" + data.ToString());
                };


                // Add logic on what to do when we get "MyFirstEvent"
                source.Dynamic.AddCallbackForProviderEvent("Microsoft-Windows-DNS-Client", "MyFirstEvent", delegate (TraceEvent data)
                {
                    firstEventTimeMSec[(int)data.PayloadByName("MyId")] = data.TimeStampRelativeMSec;
                });

                // Add logic on what to do when we get "MySecondEvent"
                source.Dynamic.AddCallbackForProviderEvent("Microsoft-Windows-DNS-Client", "MySecondEvent", delegate (TraceEvent data)
                {
                    var myId = (int)data.PayloadByName("MyId");
                    double firstEventTime;
                    if (firstEventTimeMSec.TryGetValue(myId, out firstEventTime))
                    {
                        firstEventTimeMSec.Remove(myId); // We are done with the ID after matching it, so remove it from the table. 
                        Console.WriteLine("   >>> Time Delta from first Event = {0:f3} MSec", data.TimeStampRelativeMSec - firstEventTime);
                    }
                    else
                    {
                        Console.WriteLine("   >>> WARNING, Found a 'SecondEvent' without a corresponding 'FirstEvent'");
                    }
                });

                source.Dynamic.AddCallbackForProviderEvent("Microsoft-Windows-DNS-Client", "MyStopEvent", delegate (TraceEvent data)
                {
                    Console.WriteLine("  >>> Got a stop message");

                    // Stop processing after we we see the 'Stop' event
                    source.StopProcessing();
                });

                source.Process();
                Console.WriteLine("DoneProcessing");
            }
        }
    }
}