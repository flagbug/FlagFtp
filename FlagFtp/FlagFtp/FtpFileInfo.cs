using System;

namespace FlagFtp
{
    /// <summary>
    /// Represents a file on a FTP-server
    /// </summary>
    public class FtpFileInfo : FtpFileSystemInfo
    {
        /// <summary>
        /// Gets the last write time.
        /// </summary>
        public DateTime LastWriteTime { get; private set; }

        /// <summary>
        /// Gets the length.
        /// </summary>
        public long Length { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpFileInfo"/> class.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <param name="lastWriteTime">The last write time.</param>
        /// <param name="length">The file length.</param>
        internal FtpFileInfo(Uri path, DateTime lastWriteTime, long length)
            : base(path, FtpFileSystemInfoType.File)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException("length");

            this.LastWriteTime = lastWriteTime;
            this.Length = length;
        }
    }
}