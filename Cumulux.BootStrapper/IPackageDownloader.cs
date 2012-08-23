namespace Cumulux.BootStrapper
{
    using System;

    public interface IPackageDownloader
    {
        string DownloadPackageToDisk(Uri url, bool overwrite, string localResourceDir);
    }
}
