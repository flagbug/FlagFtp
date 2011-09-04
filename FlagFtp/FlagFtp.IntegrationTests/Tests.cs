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

            Console.Write("FTP-Address: ");
            Console.Write("ftp://");

            hostAddress = "ftp://" + Console.ReadLine();

            Console.Write("Username: ");

            username = Console.ReadLine();

            Console.Write("Password: ");

            password = Console.ReadLine();

            Console.WriteLine("Starting integration test...");
            Console.WriteLine();

            client = new FtpClient(new NetworkCredential(username, password));

            CreateDirectoryTest(new Uri(new Uri(hostAddress), "/TestDirectory1"));

            Console.WriteLine("Integration test finished.");
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