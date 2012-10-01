using System;

namespace FlagFtp
{
    /// <summary>
    /// Represents a directory on a FTP-server
    /// </summary>
    public class FtpDirectoryInfo : FtpFileSystemInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FtpDirectoryInfo"/> class.
        /// </summary>
        /// <param name="path">The path of the directory.</param>
        internal FtpDirectoryInfo(Uri path)
            : base(path, FtpFileSystemInfoType.Directory)
        { }
    }
}