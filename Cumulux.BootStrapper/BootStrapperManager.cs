using System.IO;
using System.Threading;

namespace Cumulux.BootStrapper
{
    using System;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using Microsoft.WindowsAzure.Storage.Blob;

    public class BootStrapperManager
    {
        readonly IPackageDownloader downloader;
        readonly IPackageRunner runner;
        readonly IPackageUnzipper unzipper;
        private readonly IPackageUploader uploader;
        private readonly IPackageZipper zipper;

        public BootStrapperManager(ILogger logger, IPackageDownloader downloader, IPackageRunner runner, IPackageUnzipper unzipper, IPackageUploader uploader = null, IPackageZipper zipper = null)
        {
            this.downloader = downloader;
            this.runner = runner;
            this.unzipper = unzipper;
            this.uploader = uploader;
            this.zipper = zipper;
        }

        private CloudBlobClient Client 
        { 
            get;
            set;
        }

        /// <summary>
        /// Begins orchestration of bootstrapper tasks.
        /// </summary>
        /// <param name="args">The validated args parsed from the commmand line.</param>
        public void Start(BootStrapperArgs args)
        {
            if (RoleEnvironment.IsEmulated && !args.RunInEmulator)
            {
                // Skip running if under emulator
                return;
            }

            try
            {
                Client = String.IsNullOrEmpty(args.StorageConnection) ? null : CloudStorageAccount.Parse(args.StorageConnection).CreateCloudBlobClient();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("The Azure storage connection string is not valid:  " + ex.Message, args.StorageConnection);
            }
            
            //download package (if not previously downloaded).  RunAlways forces download always.
            Uri targetUri = null;
            String package = null;
            if ( !String.IsNullOrEmpty(args.Get) )
            {
                Uri urlToDownload = null;

                //if a storage account is present generate a SAS for the get
                if (Client != null)
                {
                    //x| var account = CloudStorageAccount.Parse(args.StorageConnection);
                    //x| var client = account.CreateCloudBlobClient();

                    var reference = Client.GetBlobReferenceFromServer(new Uri(args.Get.StartsWith("/") ? args.Get.Substring(1) : args.Get));
                    
                    var sas = reference.GetSharedAccessSignature(
                        new SharedAccessBlobPolicy()
                        {
                            Permissions = SharedAccessBlobPermissions.Read,
                            SharedAccessExpiryTime = DateTime.Now.AddMinutes(10)
                        });

                    urlToDownload = new Uri(reference.Uri, sas);
                }
                else //otherwise, this is a standard get
                {
                    if (!Uri.TryCreate(args.Get, UriKind.Absolute, out urlToDownload))
                        throw new ArgumentException("Invalid Uri", "args.Get");
                }

                //always download as a get
                package = this.downloader.DownloadPackageToDisk(
                    urlToDownload,
                    args.RunAlways,
                    args.LocalResource
                    );

                //Unzip without replace.  RunAlways forces unzip with replace
                if (args.Unzip)
                {
                    this.unzipper.Unzip(package, args.UnzipTarget, args.RunAlways);
                }
            }

            if ( !String.IsNullOrEmpty(args.Put) )
            {
                if ( Client != null )
                {
                    try {
                        var blob = Client.GetBlobReferenceFromServer(new Uri(args.Put.Trim('/')));
                        blob.Container.CreateIfNotExists();
                        String target = blob.GetSharedAccessSignature(
                            new SharedAccessBlobPolicy() {
                                Permissions = SharedAccessBlobPermissions.Write,
                                SharedAccessExpiryTime = DateTime.Now.AddMinutes(30)
                            });
                        if (args.Overwrite)
                            blob.DeleteIfExists();
                        targetUri = new Uri(blob.Uri, target);
                    }
                    catch ( StorageException ex ) {
                        if (Uri.IsWellFormedUriString(args.Put, UriKind.Relative))
                            throw new ArgumentException(
                                "A valid connection string to Azure blob storage must be supplied when uploading to a relative URL.\n" + ex.Message,
                                args.StorageConnection);
                    }
                }
                if ( targetUri == null ) //i| fall back on standard put.
                {
                    if ( !Uri.TryCreate(args.Put, UriKind.Absolute, out targetUri) )
                        throw new ArgumentException("Invalid URL was supplied for the PUT argument.", args.Put);
                }

                //i| Only package when we don't have a filename (didn't get), and the local resource is an existing folder.
                if ( package == null &&  Directory.Exists(args.LocalResource))
                {
                    package = Path.Combine(args.LocalResource, Path.GetFileName(targetUri.LocalPath));
                    this.zipper.Zip(args.LocalResource, package, args.Overwrite);
                }
                
                //i| Always download as a get
                package = this.uploader.UploadPackageToStorage(
                    package ?? args.LocalResource, 
                    targetUri,
                    args.Overwrite
                    );
            }

            //Run the command, record output to local disk for repository
            if (!String.IsNullOrEmpty(args.Run))
            {
                if (!this.runner.PackageExists(args.Run) || args.RunAlways)
                {
                    this.runner.Start(
                        args.Run,
                        args.Args,
                        args.EnableSystemProfile,
                        args.Block
                        );
                }
            }
        }
    }
}
