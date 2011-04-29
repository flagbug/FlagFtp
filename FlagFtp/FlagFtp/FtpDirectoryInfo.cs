using System;

namespace FlagFtp
{
    public class FtpDirectoryInfo : FtpFileSystemInfo
    {
        internal FtpDirectoryInfo(Uri path)
            : base(path, FtpFileSystemInfoType.Directory)
        { }
    }
}