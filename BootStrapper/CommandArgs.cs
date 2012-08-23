namespace BootStrapper
{
    using System;
    using System.ComponentModel;
    using Args;

    [ArgsModel(SwitchDelimiter = "-")]
    [Description("BootStrapper Help v1.1")]
    internal class CommandArgs
    {
        [Description("The full address (http://) of the file to download or relative URI when downloading from Windows Azure Blob Storage")]
        public string Get { get; set; }

        [ArgsMemberSwitch("storage", "sc")]
        [Description("Windows Azure Storage Connection String")]
        public string StorageConnection { get; set; }

        [Description("Fully qualified target URL (http://) of the upload file location or relative URI when\nuploading to Windows Azure Blob Storage.")]
        public string Put { get; set; }

        //[Description("Unzip after downloading")]
        //public bool Unzip { get; set; }

        //[ArgsMemberSwitch("target", "unzipTarget")]
        //[Description("The directory where the files will be unzipped")]
        //public string UnzipTarget { get; set; }

        [ArgsMemberSwitch("unzip")]
        [Description("The destination folder for the contents of the zip file")]
        public string Unzip { get; set; }

        [ArgsMemberSwitch("lr", "localResource")]
        [Description("The local resource path.")]
        public string LocalResource { get; set; }

        [Description("Absolute path to the executable to start.  By default, this will only run once - even on reboots")]
        public string Run { get; set; }

        [Description("Arguments to pass to the executable invoked with -run")]
        public string Args { get; set; }

        [ArgsMemberSwitch("runAlways")]
        [Description("Force to bootstrapper to always download, always unzip, and always execute the -run option")]
        public bool RunAlways { get; set; }

        [ArgsMemberSwitch("overwrite")]
        [Description("Force to bootstrapper to overwrite any existing files when download, uploading, or unpacking.")]
        public bool Overwrite { get; set; }

        [Description("Blocks the Run command until exit.  Not a good idea for starting a daemon/service")]
        public bool Block { get; set; }

        [ArgsMemberSwitch("enable", "esp")]
        [Description("Enable system profile.  This is a special mode for some installers that require a user profile location.")]
        public bool EnableSystemProfile { get; set; }

        [ArgsMemberSwitch("runInEmulator")]
        [Description("Run Bootstrapper when running under Azure Emulated environment")]
        [DefaultValue(true)]
        public bool RunInEmulator { get; set; }


        [ArgsMemberSwitch("help", "h")]
        [Description("Shows the program help")]
        [DefaultValue(true)]
        public bool ShowHelp { get; set; }

    }
}
