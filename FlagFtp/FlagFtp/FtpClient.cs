using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace FlagFtp
{
    /// <summary>
    /// Provides methods for acting on a FTP-server
    /// </summary>
    public class FtpClient
    {
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
        /// <param name="credentials">The login credentials.</param>
        public FtpClient(NetworkCredential credentials)
        {
            if (credentials == null)
                throw new ArgumentNullException("credentials");
        }

        /// <summary>
        /// Gets the directories that are contained in the specified directory.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <returns>
        /// An enumeration of <see cref="FlagFtp.FtpDirectoryInfo"/>.
        /// </returns>
        public IEnumerable<FtpDirectoryInfo> GetDirectories(Uri directory)
        {
            if (directory == null)
                throw new ArgumentNullException("directory");

            if (directory.Scheme != Uri.UriSchemeFtp)
                throw new ArgumentException("The directory isn't a valid FTP URI", "directory");

            return this.GetFileSystemInfos(directory, FtpFileSystemInfoType.Directory)
                .Cast<FtpDirectoryInfo>();
        }

        /// <summary>
        /// Gets the files that are contained in the specified directory.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <returns>
        /// An enumeration of <see cref="FlagFtp.FtpFileInfo"/>.
        /// </returns>
        public IEnumerable<FtpFileInfo> GetFiles(Uri directory)
        {
            if (directory == null)
                throw new ArgumentNullException("directory");

            if (directory.Scheme != Uri.UriSchemeFtp)
                throw new ArgumentException("The directory isn't a valid FTP URI", "directory");

            return this.GetFileSystemInfos(directory, FtpFileSystemInfoType.File)
                .Cast<FtpFileInfo>();
        }

        /// <summary>
        /// Opens the specified file for read access.
        /// </summary>
        /// <param name="file">The file to open.</param>
        /// <returns>
        /// An FTP stream to read from the file.
        /// </returns>
        public FtpStream OpenRead(FtpFileInfo file)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            WebClient client = new WebClient();
            client.Credentials = this.Credentials;

            return new FtpStream(client.OpenRead(file.Uri), file.Length);
        }

        /// <summary>
        /// Opens the specified file for read access.
        /// </summary>
        /// <param name="file">The URI of the file to open.</param>
        /// <returns>
        /// A <see cref="FlagFtp.FtpStream"/> to read from the file.
        /// </returns>
        public FtpStream OpenRead(Uri file)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            if (file.Scheme != Uri.UriSchemeFtp)
                throw new ArgumentException("The file isn't a valid FTP URI", "file");

            WebClient client = new WebClient();
            client.Credentials = this.Credentials;

            long fileSize = this.GetFileSize(file);

            return new FtpStream(client.OpenRead(file), fileSize);
        }

        /// <summary>
        /// Opens the specified file for write access.
        /// </summary>
        /// <param name="file">The file to open.</param>
        /// <returns>
        /// A <see cref="System.IO.Stream"/> to write to the file.
        /// </returns>
        public Stream OpenWrite(FtpFileInfo file)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            return this.OpenWrite(file.Uri);
        }

        /// <summary>
        /// Opens the specified file for write access.
        /// </summary>
        /// <param name="file">The URI of the file to open.</param>
        /// <returns>
        /// A <see cref="System.IO.Stream"/> to write to the file.
        /// </returns>
        public Stream OpenWrite(Uri file)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            if (file.Scheme != Uri.UriSchemeFtp)
                throw new ArgumentException("The file isn't a valid FTP URI", "file");

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(file);
            request.Credentials = this.Credentials;
            request.Method = WebRequestMethods.Ftp.UploadFile;

            return request.GetRequestStream();
        }

        /// <summary>
        /// Deletes the specified FTP file.
        /// </summary>
        /// <param name="file">The file to delete.</param>
        public void DeleteFile(FtpFileInfo file)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            this.DeleteFile(file.Uri);
        }

        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="file">The URI of the file to delete.</param>
        public void DeleteFile(Uri file)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            if (file.Scheme != Uri.UriSchemeFtp)
                throw new ArgumentException("The file isn't a valid FTP URI", "file");

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(file);
            request.Credentials = this.Credentials;
            request.Method = WebRequestMethods.Ftp.DeleteFile;

            using (var response = request.GetResponse()) { }
        }

        /// <summary>
        /// Creates the specified directory on the FTP server.
        /// </summary>
        /// <param name="directory">The URI of the directory to create.</param>
        public void CreateDirectory(Uri directory)
        {
            if (directory == null)
                throw new ArgumentNullException("directory");

            if (directory.Scheme != Uri.UriSchemeFtp)
                throw new ArgumentException("The directory isn't a valid FTP URI", "directory");

            WebRequest request = WebRequest.Create(directory);
            request.Method = WebRequestMethods.Ftp.MakeDirectory;
            request.Credentials = this.Credentials;

            using (var response = request.GetResponse()) { }
        }

        /// <summary>
        /// Deletes the specified FTP directory.
        /// </summary>
        /// <param name="directory">The directory to delete.</param>
        public void DeleteDirectory(FtpDirectoryInfo directory)
        {
            if (directory == null)
                throw new ArgumentNullException("directory");

            this.DeleteDirectory(directory.Uri);
        }

        /// <summary>
        /// Deletes the specified directory.
        /// </summary>
        /// <param name="directory">The URI of the directory to delete.</param>
        public void DeleteDirectory(Uri directory)
        {
            if (directory == null)
                throw new ArgumentNullException("directory");

            if (directory.Scheme != Uri.UriSchemeFtp)
                throw new ArgumentException("The directory isn't a valid FTP URI", "directory");

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(directory);
            request.Credentials = this.Credentials;
            request.Method = WebRequestMethods.Ftp.RemoveDirectory;

            request.GetResponse();
        }

        /// <summary>
        /// Gets the <see cref="FlagFtp.FtpFileInfo"/> for the specified URI.
        /// </summary>
        /// <param name="file">The URI of the file.</param>
        /// <returns>
        /// A <see cref="FlagFtp.FtpFileInfo"/> that contains informations about the file.
        /// </returns>
        public FtpFileInfo GetFileInfo(Uri file)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            if (file.Scheme != Uri.UriSchemeFtp)
                throw new ArgumentException("The file isn't a valid FTP URI", "file");

            DateTime lastWriteTime = this.GetTimeStamp(file);
            long length = this.GetFileSize(file);

            return new FtpFileInfo(file, lastWriteTime, length);
        }

        /// <summary>
        /// Gets the <see cref="FlagFtp.FtpDirectoryInfo"/> for the specified URI.
        /// </summary>
        /// <param name="directory">The URI of the directory.</param>
        /// <returns>
        /// A <see cref="FlagFtp.FtpDirectoryInfo"/> that contains informations about the directory.
        /// </returns>
        public FtpDirectoryInfo GetDirectoryInfo(Uri directory)
        {
            if (directory == null)
                throw new ArgumentNullException("directory");

            if (directory.Scheme != Uri.UriSchemeFtp)
                throw new ArgumentException("The directory isn't a valid FTP URI", "directory");

            return new FtpDirectoryInfo(directory);
        }

        /// <summary>
        /// Determines if the specified file exists on the FTP server.
        /// </summary>
        /// <param name="file">The URI of the file.</param>
        /// <returns>
        /// True, if the file exists; otherwise false.
        /// </returns>
        public bool FileExists(Uri file)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            if (file.Scheme != Uri.UriSchemeFtp)
                throw new ArgumentException("The file isn't a valid FTP URI", "file");

            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(file);
            request.Credentials = this.Credentials;
            request.Method = WebRequestMethods.Ftp.GetDateTimestamp;

            try
            {
                using (var response = request.GetResponse()) { }
            }

            catch (WebException ex)
            {
                using (var response = (FtpWebResponse)ex.Response)
                {
                    if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    {
                        return false;
                    }

                    else
                    {
                        throw;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Determines if the specified directory exists on the FTP server.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <returns>
        /// True, if the directory exists; otherwise false.
        /// </returns>
        public bool DirectoryExists(Uri directory)
        {
            if (directory == null)
                throw new ArgumentNullException("directory");

            if (directory.Scheme != Uri.UriSchemeFtp)
                throw new ArgumentException("The directory isn't a valid FTP URI", "directory");

            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(directory);
            request.Credentials = this.Credentials;
            request.Method = WebRequestMethods.Ftp.ListDirectory;

            try
            {
                using (var response = request.GetResponse()) { }
            }

            catch (WebException ex)
            {
                using (var response = (FtpWebResponse)ex.Response)
                {
                    if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    {
                        return false;
                    }

                    else
                    {
                        throw;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Gets the files or directories from the specified directory.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="type">The type.</param>
        /// <returns>
        /// An enumeration of <see cref="FlagFtp.FtpFileSystemInfo"/> with the type of the specified type argument.
        /// </returns>
        private IEnumerable<FtpFileSystemInfo> GetFileSystemInfos(Uri directory, FtpFileSystemInfoType type)
        {
            if (directory == null)
                throw new ArgumentNullException("directory");

            if (directory.Scheme != Uri.UriSchemeFtp)
                throw new ArgumentException("The directory isn't a valid FTP URI", "directory");

            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(directory);
            request.Credentials = this.Credentials;
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

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
                                    FullName = new Uri(new Uri(directory + "/"), match.Groups["Name"].Value)
                                })
                            .Where(info => info.Name != "." && info.Name != "..")
                            .ToList();

                        switch (type)
                        {
                            case FtpFileSystemInfoType.Directory:
                                return infos.Where(info => info.IsDirectory)
                                    .Select(info => new FtpDirectoryInfo(info.FullName))
                                    .Cast<FtpFileSystemInfo>();

                            case FtpFileSystemInfoType.File:
                                return infos.Where(info => !info.IsDirectory)
                                    .Select(info => new FtpFileInfo(info.FullName, this.GetTimeStamp(info.FullName), info.FileLength))
                                    .Cast<FtpFileSystemInfo>();
                        }
                    }
                }
            }

            throw new InvalidOperationException("Method should not reach this code!");
        }

        /// <summary>
        /// Gets the time stamp for the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>
        /// A <see cref="System.DateTime"/> object the represents the last write time of the file.
        /// </returns>
        private DateTime GetTimeStamp(Uri file)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            if (file.Scheme != Uri.UriSchemeFtp)
                throw new ArgumentException("The file isn't a valid FTP URI", "file");

            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(file);

            request.Method = WebRequestMethods.Ftp.GetDateTimestamp;
            request.Credentials = this.Credentials;

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                return response.LastModified;
            }
        }

        /// <summary>
        /// Gets the size of the specified file in bytes.
        /// </summary>
        /// <param name="file">The file URI.</param>
        /// <returns>
        /// The file size in bytes.
        /// </returns>
        private long GetFileSize(Uri file)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            if (file.Scheme != Uri.UriSchemeFtp)
                throw new ArgumentException("The file isn't a valid FTP URI", "file");

            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(file);

            request.Method = WebRequestMethods.Ftp.GetFileSize;
            request.Credentials = this.Credentials;

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                return response.ContentLength;
            }
        }
    }
}