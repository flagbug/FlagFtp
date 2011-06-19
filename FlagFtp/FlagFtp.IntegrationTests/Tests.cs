using System;
using System.Net;

namespace FlagFtp.IntegrationTests
{
    internal class Tests
    {
        private static FtpClient client;

        private static void Main(string[] args)
        {
            string hostAddress;
            string username;
            string password;

            Console.WriteLine("FTP-Address: ");
            Console.Write("ftp://");

            hostAddress = "ftp://" + Console.ReadLine();

            Console.WriteLine("Username: ");

            username = Console.ReadLine();

            Console.WriteLine("Password: ");

            password = Console.ReadLine();

            client = new FtpClient(new NetworkCredential(username, password));

            CreateDirectoryTest(new Uri(new Uri(hostAddress), "/TestDirectory1"));

            Console.WriteLine("Finished all tests.");
            Console.ReadLine();
        }

        private static void CreateDirectoryTest(Uri directory)
        {
            client.CreateDirectory(directory);

            bool exists = client.DirectoryExists(directory);
            Console.WriteLine("CreateDirectory method has " + (exists ? "SUCCEED" : "FAILED"));
        }
    }
}