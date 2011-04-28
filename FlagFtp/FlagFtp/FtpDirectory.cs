using System.IO;

namespace FlagFtp
{
    public class FtpDirectory
    {
        /// <summary>
        /// Gets the full name of the directory.
        /// </summary>
        public string FullName { get; private set; }

        public string Name
        {
            get
            {
                return Path.GetDirectoryName(this.FullName);
            }
        }
    }
}