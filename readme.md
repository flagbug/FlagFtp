# FlagFtp

## Overview

FlagFtp is an FTP library for .NET, that supports various operations, such as retrieving file lists, 
write and read from/to files, retrieving file and directory infos, etc...

## NuGet

FlagConsole is available via NuGet!
http://www.nuget.org/List/Packages/FlagFtp

## Example code

*Retrieving a file list*

```c#

var credential = new NetworkCredential("username", "password");

var client = new FtpClient(credential);

IEnumerable<FtpFileInfo> files = client.GetFiles(new Uri("ftp://yourserver/yourdirectory/"));

foreach(var file in files)
{
    Console.WriteLine(file.Name);
    Console.WriteLine(file.Length);
    Console.WriteLine(file.LastWriteTime);
}

```
    
**Projects that use this library:**

- [FlagSync](http://github.com/flagbug/FlagSync)
