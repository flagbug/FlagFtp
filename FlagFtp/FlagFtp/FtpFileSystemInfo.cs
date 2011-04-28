using System;
using System.IO;

namespace FlagFtp
{
    public enum FileSystemInfoType
    {
        File,
        Directory
    }

    public abstract class FtpFileSystemInfo
    {
        /// <summary>
        /// Gets the full name.
        /// </summary>
        public string FullName { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name
        {
            get
            {
                return Path.GetDirectoryName(this.FullName);
            }
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public FileSystemInfoType Type { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpFileSystemInfo"/> class.
        /// </summary>
        /// <param name="fullName">The full name.</param>
        /// <param name="type">The type.</param>
        protected FtpFileSystemInfo(Uri fullName, FileSystemInfoType type)
        {
            if (fullName == null)
                throw new ArgumentNullException("fullName");

            if (fullName.Scheme != Uri.UriSchemeFtp)
                throw new ArgumentException("The name isn't a valid FTP Uri", "host");

            this.FullName = fullName.OriginalString;
            this.Type = type;
        }
    }
}