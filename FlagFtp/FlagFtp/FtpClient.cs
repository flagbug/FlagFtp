using System;
using System.Net;

namespace FlagFtp
{
    public class FtpClient
    {
        /// <summary>
        /// Gets the host.
        /// </summary>
        public Uri Host { get; private set; }

        /// <summary>
        /// Gets or sets the credentials.
        /// </summary>
        /// <value>
        /// The credentials.
        /// </value>
        public NetworkCredential Credentials { get; set; }

        /// <summary>
        /// Gets the current working directory.
        /// </summary>
        public FtpDirectory CurrentDirectory { get; private set; }

        public FtpClient(Uri host)
        {
            if (host == null)
                throw new ArgumentNullException("host");

            if (host.Scheme != Uri.UriSchemeFtp)
                throw new ArgumentException("The host isn't a valid FTP Uri", "host");
        }
    }
}