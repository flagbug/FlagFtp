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

            this.Credentials = credentials;
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

            directory = this.NormalizeUri(directory);

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

            directory = this.NormalizeUri(directory);

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

            return new FtpStream(this.CreateClient().OpenRead(file.Uri), file.Length);
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

            file = this.NormalizeUri(file);

            long fileSize = this.GetFileSize(file);

            return new FtpStream(this.CreateClient().OpenRead(file), fileSize);
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

            file = this.NormalizeUri(file);

            return this.CreateRequest(file, WebRequestMethods.Ftp.UploadFile)
                .GetRequestStream();
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

            file = this.NormalizeUri(file);

            using (this.CreateResponse(file, WebRequestMethods.Ftp.DeleteFile))
            { }
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

            directory = this.NormalizeUri(directory);

            using (this.CreateResponse(directory, WebRequestMethods.Ftp.MakeDirectory))
            { }
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

            directory = this.NormalizeUri(directory);

            using (this.CreateResponse(directory, WebRequestMethods.Ftp.RemoveDirectory))
            { }
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

            file = this.NormalizeUri(file);

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

            directory = this.NormalizeUri(directory);

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

            file = this.NormalizeUri(file);

            var files = this.GetFiles(new Uri(file, "./"));

            return files.Any(f => this.NormalizeUri(f.Uri).AbsoluteUri == file.AbsoluteUri);
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

            directory = this.NormalizeUri(directory);

            var directories = this.GetDirectories(new Uri(directory, "./"));

            return directories.Any(dir => this.NormalizeUri(dir.Uri).AbsoluteUri == directory.AbsoluteUri);
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

            using (var response = (this.CreateResponse(directory, WebRequestMethods.Ftp.ListDirectoryDetails)))
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(responseStream))
                    {
                        string all = reader.ReadToEnd();

                        var regex = new Regex(@"^(?<FileOrDirectory>[d-])(?<Attributes>[rwxts-]{3}){3}\s+\d{1,}\s+.*?(?<FileSize>\d{1,})\s+(?<Date>\w+\s+\d{1,2}\s+(?:\d{4})?)(?<YearOrTime>\d{1,2}:\d{2})?\s+(?<Name>.+?)\s?$",
                            RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

                        MatchCollection matches = regex.Matches(all);

                        var infos = matches.Cast<Match>()
                            .Select(
                                match =>
                                new
                                {
                                    IsDirectory = match.Groups["FileOrDirectory"].Value == "d",
                                    FileLength = long.Parse(match.Groups["FileSize"].Value),
                                    Name = match.Groups["Name"].Value,
                                    FullName = new Uri(new Uri(directory + "/"), match.Groups["Name"].Value)
                                })
                            .Where(info => info.Name != "." && info.Name != "..");

                        switch (type)
                        {
                            case FtpFileSystemInfoType.Directory:
                                return infos.Where(info => info.IsDirectory)
                                    .Select(info => new FtpDirectoryInfo(this.NormalizeUri(info.FullName)))
                                    .Cast<FtpFileSystemInfo>();

                            case FtpFileSystemInfoType.File:
                                return infos.Where(info => !info.IsDirectory)
                                    .Select(info => new FtpFileInfo(this.NormalizeUri(info.FullName), this.GetTimeStamp(info.FullName), info.FileLength))
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

            file = this.NormalizeUri(file);

            using (var response = this.CreateResponse(file, WebRequestMethods.Ftp.GetDateTimestamp))
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

            file = this.NormalizeUri(file);

            using (var response = this.CreateResponse(file, WebRequestMethods.Ftp.GetFileSize))
            {
                return response.ContentLength;
            }
        }

        /// <summary>
        /// Creates a <see cref="System.Net.FtpWebResponse"/> from the specified request URI, request method and the necessary credentials.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="requestMethod">The request method.</param>
        /// <returns>
        /// A <see cref="System.Net.FtpWebRequest"/> with the specified request uri, request method and the necessary credentuials.
        /// </returns>
        private FtpWebResponse CreateResponse(Uri requestUri, string requestMethod)
        {
            if (requestUri == null)
                throw new ArgumentNullException("requestUri");

            if (requestUri.Scheme != Uri.UriSchemeFtp)
                throw new ArgumentException("The request URI isn't a valid FTP URI", "requestUri");

            requestUri = this.NormalizeUri(requestUri);

            return (FtpWebResponse)this.CreateRequest(requestUri, requestMethod).GetResponse();
        }

        /// <summary>
        /// Creates a <see cref="System.Net.FtpWebRequest"/> from the specified request URI, request method and the necessary credentials.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="requestMethod">The request method.</param>
        /// <returns>
        /// A <see cref="System.Net.FtpWebRequest"/> with the specified request uri, request method and the necessary credentuials.
        /// </returns>
        private FtpWebRequest CreateRequest(Uri requestUri, string requestMethod)
        {
            if (requestUri == null)
                throw new ArgumentNullException("requestUri");

            if (requestUri.Scheme != Uri.UriSchemeFtp)
                throw new ArgumentException("The request URI isn't a valid FTP URI", "requestUri");

            requestUri = this.NormalizeUri(requestUri);

            var request = (FtpWebRequest)WebRequest.Create(requestUri);

            request.Method = requestMethod;
            request.Credentials = this.Credentials;

            return request;
        }

        /// <summary>
        /// Creates a <see cref="System.Net.WebClient"/> with the necessary credentials.
        /// </summary>
        /// <returns>
        /// A <see cref="System.Net.WebClient"/> with the necessary credentials
        /// </returns>
        private WebClient CreateClient()
        {
            var client = new WebClient { Credentials = this.Credentials };

            return client;
        }

        /// <summary>
        /// Normalizes the URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>A normalized URI</returns>
        private Uri NormalizeUri(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");

            if (uri.Scheme != Uri.UriSchemeFtp)
                throw new ArgumentException("The URI isn't a valid FTP URI", "uri");

            string path = uri.AbsoluteUri;

            //Cut the "ftp://" off
            path = path.Substring(6);

            path = path.Replace("//", "/").Replace(@"\\", "/").Replace(@"\", "/");

            return new Uri("ftp://" + path);
        }
    }
}