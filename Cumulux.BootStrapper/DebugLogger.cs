namespace Cumulux.BootStrapper
{
    using System;
    using System.Diagnostics;

    public class DebugLogger : ILogger
    {
        public void WriteLine(string message)
        {
            Debug.WriteLine(message);
        }

        public void WriteLine(string format, params object[] args)
        {
            Debug.WriteLine(format, args);
        }
    }
}
