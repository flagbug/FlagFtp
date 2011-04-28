using System;

namespace FlagFtp
{
    public class FtpDirectory : FtpFileSystemInfo
    {
        public FtpDirectory(Uri path)
            : base(path, FileSystemInfoType.Directory)
        { }
    }
}