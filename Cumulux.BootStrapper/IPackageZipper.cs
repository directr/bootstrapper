using System;
using System.IO;

namespace Cumulux.BootStrapper
{
    public interface IPackageZipper
    {
        void Zip(String zipDir, String packageFile, Boolean overwrite);
    }
}
