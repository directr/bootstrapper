using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Cumulux.BootStrapper
{
    public class TraceLogger : ILogger
    {
        public void WriteLine(string message)
        {
            Trace.WriteLine(message);
        }

        public void WriteLine(string format, params object[] args)
        {
            Trace.WriteLine(String.Format(format, args));
        }

    }
}
