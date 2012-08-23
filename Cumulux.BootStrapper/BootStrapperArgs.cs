using Microsoft.WindowsAzure;

namespace Cumulux.BootStrapper
{
    using System;

    public class BootStrapperArgs
    {
        public bool EnableSystemProfile { get; set; }
        public bool Unzip { get; set; }
        public bool RunAlways { get; set; }
        public bool Block { get; set; }
        public string Get { get; set; }
        public string Run { get; set; }
        public string Args { get; set; }
        public string LocalResource { get; set; }
        public string UnzipTarget { get; set; }
        public string StorageConnection { get; set; }
        public string Put { get; set; }
        public bool Overwrite { get; set; }
        public bool RunInEmulator { get; set; }
    }
}
