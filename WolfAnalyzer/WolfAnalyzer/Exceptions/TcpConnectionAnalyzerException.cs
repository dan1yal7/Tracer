

namespace WolfAnalyzer.Exceptions
{
    public class TcpConnectionAnalyzerException : Exception
    { 
        public TcpConnectionAnalyzerException()
        {

        }
        public TcpConnectionAnalyzerException(string message) 
           : base (message)
        {

        }
        public TcpConnectionAnalyzerException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
