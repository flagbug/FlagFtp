using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

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
        /// Initializes a new instance of the <see cref="FtpClient"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        public FtpClient(Uri host)
        {
            if (host == null)
                throw new ArgumentNullException("host");

            if (host.Scheme != Uri.UriSchemeFtp)
                throw new ArgumentException("The host isn't a valid FTP Uri", "host");
        }

        /// <summary>
        /// Gets the directories that are contained in the specified directory.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <returns></returns>
        public IEnumerable<FtpDirectory> GetDirectories(Uri directory)
        {
            return this.GetFileSystemInfos(directory, FtpFileSystemInfoType.Directory)
                .Cast<FtpDirectory>();
        }

        /// <summary>
        /// Gets the files or directories from the specified directory.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private IEnumerable<FtpFileSystemInfo> GetFileSystemInfos(Uri directory, FtpFileSystemInfoType type)
        {
            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(directory);

            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            request.Credentials = this.Credentials;

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        string all = reader.ReadToEnd();

                        Regex regex = new Regex(@"^(?<FileOrDirectory>[d-])(?<Attributes>[rwxt-]{3}){3}\s+\d{1,}\s+.*?(?<FileSize>\d{1,})\s+(?<Date>\w+\s+\d{1,2}\s+(?:\d{4})?)(?<YearOrTime>\d{1,2}:\d{2})?\s+(?<Name>.+?)\s?$",
                            RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

                        MatchCollection matches = regex.Matches(all);

                        var infos = matches.Cast<Match>()
                            .Select(
                                match =>
                                new
                                {
                                    IsDirectory = match.Groups["FileOrDirectory"].Value == "d" ? true : false,
                                    FileLength = long.Parse(match.Groups["FileSize"].Value),
                                    Name = match.Groups["Name"].Value,
                                    FullName = new Uri(new Uri(directory.AbsoluteUri), match.Groups["Name"].Value)
                                })
                            .Where(info => info.Name != "." && info.Name != "..")
                            .ToList();

                        if (type == FtpFileSystemInfoType.Directory)
                        {
                            return infos.Where(info => info.IsDirectory)
                                .Select(info => new FtpDirectory(info.FullName))
                                .Cast<FtpFileSystemInfo>();
                        }

                        else
                        {
                            return infos.Where(info => !info.IsDirectory)
                                .Select(info => new FtpFile(info.FullName, this.GetTimeStamp(info.FullName), info.FileLength))
                                .Cast<FtpFileSystemInfo>();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the time stamp for the specified file.
        /// </summary>
        /// <param name="files">The files.</param>
        /// <returns></returns>
        private DateTime GetTimeStamp(Uri file)
        {
            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(file);

            request.Method = WebRequestMethods.Ftp.GetDateTimestamp;
            request.Credentials = this.Credentials;

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                return response.LastModified;
            }
        }
    }
}