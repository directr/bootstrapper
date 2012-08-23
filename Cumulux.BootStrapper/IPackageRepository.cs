namespace Cumulux.BootStrapper
{
    using System;

    public interface IPackageRepository
    {
        void AddPackage(string packageName);
        bool PackageExists(string packageName);
    }
}
