using System;
using System.IO;

namespace FlagFtp
{
    public enum FtpFileSystemInfoType
    {
        File,
        Directory
    }

    public abstract class FtpFileSystemInfo
    {
        /// <summary>
        /// Gets the full name.
        /// </summary>
        public string FullName
        {
            get
            {
                return this.Uri.AbsoluteUri;
            }
        }

        /// <summary>
        /// Gets the URI.
        /// </summary>
        public Uri Uri { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name
        {
            get
            {
                return Path.GetFileName(this.FullName);
            }
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public FtpFileSystemInfoType Type { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpFileSystemInfo"/> class.
        /// </summary>
        /// <param name="fullName">The full name.</param>
        /// <param name="type">The type.</param>
        protected internal FtpFileSystemInfo(Uri uri, FtpFileSystemInfoType type)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");

            if (uri.Scheme != Uri.UriSchemeFtp)
                throw new ArgumentException("The name isn't a valid FTP Uri", "uri");

            this.Uri = uri;
            this.Type = type;
        }
    }
}