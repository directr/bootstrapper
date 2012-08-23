namespace Cumulux.BootStrapper
{
    using System;

    public class PackageRepository : IPackageRepository
    {
        public PackageRepository()
        {
        }

        public void AddPackage(string packageName)
        {
        }

        public bool PackageExists(string packageName)
        {
            return false;
        }

        private void LoadPackages()
        {
        }
    }
}
