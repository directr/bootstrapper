using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ionic.Zip;

namespace Cumulux.BootStrapper
{
    public class PackageZipper : IPackageZipper
    {
        private ILogger logger;

        public PackageZipper() : this(new NullLogger())
        {
        }

        public PackageZipper(ILogger logger)
        {
            this.logger = logger;
        }

        public void Zip(String zipDir, String packageFile, Boolean overwrite)
        {
            try
            {
                this.logger.WriteLine("Creating bootstapper zip package '{0}' from contents of directory '{1}'.", packageFile, zipDir);
                using (ZipFile zip = new ZipFile())
                {
                    zip.AddDirectory(zipDir);
                    zip.Comment = "Windows Azure Bootstrapper package.";
                    zip.Save(packageFile);
                }
            }
            catch (Exception ex)
            {
                this.logger.WriteLine("An error occurred packaging the directory.{0}{1}", Environment.NewLine, ex.ToString());
                throw;
            }
        }
    }
}
