using System;

namespace FlagFtp
{
    public class FtpDirectory : FtpFileSystemInfo
    {
        public FtpDirectory(Uri path)
            : base(path, FtpFileSystemInfoType.Directory)
        { }
    }
}