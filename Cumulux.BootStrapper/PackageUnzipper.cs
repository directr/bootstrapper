namespace Cumulux.BootStrapper
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Ionic.Zip;

    public class PackageUnzipper : IPackageUnzipper
    {
        private ILogger logger;

        public PackageUnzipper() : this(new NullLogger())
        {
        }

        public PackageUnzipper(ILogger logger)
        {
            this.logger = logger;
        }

        public void Unzip(string zipPath, string targetDir, bool overwrite)
        {
            try
            {
                using (var zipfile = ZipFile.Read(zipPath))
                {
                    foreach (ZipEntry e in zipfile)
                    {
                        e.Extract(targetDir, overwrite ? ExtractExistingFileAction.OverwriteSilently : ExtractExistingFileAction.DoNotOverwrite);
                        this.logger.WriteLine("Extracting {0}", e.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.WriteLine("An error occured during extraction:  {0}.", ex.ToString());
                throw;
            }
        }
    }
}
