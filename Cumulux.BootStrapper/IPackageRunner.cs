namespace Cumulux.BootStrapper
{
    using System;

    public interface IPackageRunner
    {
        void Start(string fileName, string args, bool enableSystemProfile, bool block);
        bool PackageExists(string fileName);
    }
}
