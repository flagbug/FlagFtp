FlagFtp
=======

Overview
--------
FlagFtp is a FTP library for .NET which supports various operations, such as retrieving file lists, 
write and read from/to files, retrieving file and directory infos, etc...

NuGet
-----
FlagConsole is available via NuGet!
http://www.nuget.org/List/Packages/FlagFtp

Example code
------------

*Retrieving a file list*

    var credentials = new NetworkCredentials("username", "password");
    
    var client = new FtpClient(credentials);
    
    IEnumerable<FtpFileInfo> files = client.GetFiles(new Uri("ftp://yourserver/yourdirectory/"));
    
    foreach(var file in files)
    {
        Console.WriteLine(file.Name);
        Console.WriteLine(file.Length);
        Console.WriteLine(file.LastWriteTime);
    }