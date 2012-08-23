namespace Cumulux.BootStrapper
{
    using System;

    public class NullLogger : ILogger
    {
        public void WriteLine(string message)
        {
        }

        public void WriteLine(string format, params object[] args)
        {
        }
    }
}
