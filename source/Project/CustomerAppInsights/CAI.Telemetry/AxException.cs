using System;

namespace CAI.Telemetry
{
    public class AxException : Exception
    {
        string _stackTrace;
        public AxException(string message, string stackTrace) : base(message)
        {
            _stackTrace = stackTrace;
        }
        public override string StackTrace => _stackTrace;
    }
}
