

namespace WolfAnalyzer.Exceptions
{
    public class AnalyzerException : Exception
    { 
        public AnalyzerException()
        {

        }
        public AnalyzerException(string message)
            : base(message)
        {

        }
        public AnalyzerException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
