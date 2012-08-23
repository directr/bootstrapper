using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace Cumulux.BootStrapper
{
    public class PackageUploader : IPackageUploader
    {
        private ILogger logger;

        public PackageUploader() : this(new NullLogger())
        {
        }

        public PackageUploader(ILogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Uploads a package to Windows Azure blob storage.
        /// </summary>
        /// <param name="source">The source Uri.  May be a local file or web URL.</param>
        /// <param name="target">The target Uri in blob storage.</param>
        /// <param name="overwrite">if set to <c>true</c> overwrite any existing blob at the target location.</param>
        /// <returns></returns>
        public string UploadPackageToStorage(String source, Uri target, Boolean overwrite)
        {
            if (!File.Exists(source))
                this.logger.WriteLine("File not found. An invalid local file supplied for upload '{0}'.", source);
            try
            {
                this.logger.WriteLine("Uploading package '{0}' to '{1}'.", source, target);
                using (var client = new WebClient())
                {
                    client.UploadFile(target, source);
                }
            }
            catch (Exception ex)
            {
                this.logger.WriteLine("An error occurred during upload", ex.Message);
                throw;
            }
            return target.ToString();
        }
    }
}
