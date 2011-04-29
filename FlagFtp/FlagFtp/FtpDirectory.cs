using System;

namespace FlagFtp
{
    public class FtpDirectory : FtpFileSystemInfo
    {
        internal FtpDirectory(Uri path)
            : base(path, FtpFileSystemInfoType.Directory)
        { }
    }
}