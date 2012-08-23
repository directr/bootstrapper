namespace Cumulux.BootStrapper
{
    using System;

    public interface ILogger
    {
        void WriteLine(string message);
        void WriteLine(string format, params object[] args);
    }
}
