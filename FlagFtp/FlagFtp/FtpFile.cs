using System;

namespace FlagFtp
{
    public class FtpFile : FtpFileSystemInfo
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
        /// Initializes a new instance of the <see cref="FtpFile"/> class.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <param name="lastWriteTime">The last write time.</param>
        /// <param name="length">The file length.</param>
        public FtpFile(Uri path, DateTime lastWriteTime, long length)
            : base(path, FileSystemInfoType.File)
        {
            this.LastWriteTime = lastWriteTime;
            this.Length = length;
        }
    }
}