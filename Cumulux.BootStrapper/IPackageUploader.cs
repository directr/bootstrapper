//i|
//i|  (i|Ryan D. Marshall|2011)
//i|
using System;

namespace Cumulux.BootStrapper
{
    /// <summary>
    /// Interface for package upload.
    /// </summary>
    public interface IPackageUploader
    {
        /// <summary>
        /// Uploads a package to Windows Azure blob storage.
        /// </summary>
        /// <param name="source">The source Uri.  May be a local file or web URL.</param>
        /// <param name="target">The target Uri in blob storage.</param>
        /// <param name="overwrite">if set to <c>true</c> overwrite any existing blob at the target location.</param>
        /// <returns></returns>
        string UploadPackageToStorage(String source, Uri target, Boolean overwrite);
    }
}
