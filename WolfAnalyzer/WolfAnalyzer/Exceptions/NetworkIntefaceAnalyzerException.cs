

namespace WolfAnalyzer.Exceptions
{
    public class NetworkIntefaceAnalyzerException : Exception
    { 
        public NetworkIntefaceAnalyzerException()
        {

        }
        public NetworkIntefaceAnalyzerException(string message)
            : base(message)
        {

        }
        public NetworkIntefaceAnalyzerException(string message, Exception inner) 
            : base(message, inner)
        {
            
        }
    }
}
