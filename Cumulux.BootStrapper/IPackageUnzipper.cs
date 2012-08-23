namespace Cumulux.BootStrapper
{
    using System;

    public interface IPackageUnzipper
    {
        void Unzip(string zipFile, string targetDir, bool overwrite);
    }
}
