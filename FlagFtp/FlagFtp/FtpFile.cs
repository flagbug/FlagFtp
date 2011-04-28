using System;
using System.IO;

namespace FlagFtp
{
    public class FtpFile
    {
        /// <summary>
        /// Gets the last write time.
        /// </summary>
        public DateTime LastWriteTime { get; private set; }

        /// <summary>
        /// Gets the full name of the file.
        /// </summary>
        public string FullName { get; private set; }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        public string Name
        {
            get
            {
                return Path.GetDirectoryName(this.FullName);
            }
        }
    }
}