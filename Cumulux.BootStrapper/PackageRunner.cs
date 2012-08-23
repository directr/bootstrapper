namespace Cumulux.BootStrapper
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Microsoft.Win32;

    public class PackageRunner : IPackageRunner
    {
        private ILogger logger;

        public PackageRunner() : this(new NullLogger())
        {
        }

        public PackageRunner(ILogger logger)
        {
            this.logger = logger;
        }

        public void Start(string fileName, string args, bool enableSystemProfile, bool block)
        {
            if (enableSystemProfile)
            {
                EnableSystemProfile();
            }

            this.logger.WriteLine("Starting {0} with args: {1}.  System Profile is{2} enabled", fileName, args, enableSystemProfile ? String.Empty : " not");

            ProcessStartInfo info = new ProcessStartInfo()
            {
                FileName = fileName,
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true, 
                //WorkingDirectory = Directory.
            };

            var p = new Process() { StartInfo = info, EnableRaisingEvents = true };

            //TODO:  think about this more in terms of process lifetime.  Logging stdout and stderr
            //for long running daemons might not be a good idea.  Might need to start separate thread.
            DataReceivedEventHandler outputHandler = (s, e) =>
            {
                File.AppendAllText("output.log", e.Data);
            };

            DataReceivedEventHandler errorHandler = (s, e) =>
            {
                File.AppendAllText("error.log", e.Data);
            };

            p.ErrorDataReceived += errorHandler;
            p.OutputDataReceived += outputHandler;

            if (enableSystemProfile)
                p.Exited += new EventHandler(p_Exited);

            p.Start();
            p.BeginErrorReadLine();
            p.BeginOutputReadLine();

            var packageLog = String.Format("{0}{1}", fileName, ".log.html");
            using (var fs = File.CreateText(packageLog))
            {
                //nothing, just create file.
                fs.WriteLine("Created");
                fs.Flush();
            }

            if (block) //blocking request
                p.WaitForExit();
        }

        private void p_Exited(object sender, EventArgs e)
        {
            this.logger.WriteLine("Resetting system profile");
            Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\User Shell Folders", true)
                .SetValue("Local AppData", Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), "AppData\\Local"));
        }

        private void EnableSystemProfile()
        {
            this.logger.WriteLine("Enabling system profile");
            var dir = Directory.CreateDirectory("AppData");
            Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\User Shell Folders", true)
                .SetValue("Local AppData", dir.FullName);
        }

        public bool PackageExists(string fileName)
        {
            return File.Exists(String.Format("{0}{1}", fileName, ".log.html"));
        }

    }
}
