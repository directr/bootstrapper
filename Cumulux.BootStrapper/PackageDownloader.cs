namespace Cumulux.BootStrapper
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading;

    public class PackageDownloader : IPackageDownloader
    {
        private ILogger logger;

        public PackageDownloader() : this(new NullLogger())
        {
        }

        public PackageDownloader(ILogger logger)
        {
            this.logger = logger;
        }

        public string DownloadPackageToDisk(Uri url, bool overwrite, string localResourceDir)
        {
            logger.WriteLine("Downloading from {0} to {1}:  Overwrite: {2}", url, localResourceDir, overwrite);

            //i| Determine if local resource is a file; otherwise it will be set downstream.
            //i| (The validity of the string as a path has been previously validated).
            String filePath= null;
            if ( File.Exists(localResourceDir) || ( Directory.Exists(Path.GetDirectoryName(localResourceDir)) && !String.IsNullOrEmpty(Path.GetExtension(localResourceDir)) ) )
                filePath= Path.GetFullPath(localResourceDir);

            if (filePath== null)
            {
                if (!Directory.Exists(localResourceDir))
                {
                    logger.WriteLine("Directory does not exist. Creating...");
                    Directory.CreateDirectory(localResourceDir);
                }
                
                //i| if the local resource was a directory, append the filename from Get.
                filePath= Path.Combine(localResourceDir, Path.GetFileName(url.LocalPath));
            }
            
            if (File.Exists(filePath))
            {
                if (!overwrite)
                {
                    logger.WriteLine("File already exists at the target location. Using previously downloaded version.\n\nRun with the -runAlways switch to force downloads.");
                    return filePath;
                }
                File.Delete(filePath);
            }
            try
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(url, filePath);
                }
            }
            catch (Exception ex)
            {
                this.logger.WriteLine(ex.Message);
                throw;
            }
            return filePath;
        }
    }
}
