using System;
using System.IO;
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

            Console.WriteLine("Reading login data...");

            /*
             * Read the login data for the server from a text-file called "Login",
             * with the following structure:
             *
             * Server address
             * Username
             * Password
             *
             */
            var loginData = File.ReadAllLines("Login");

            hostAddress = loginData[0];
            username = loginData[1];
            password = loginData[2];

            Console.WriteLine("Starting integration test...");
            Console.WriteLine();

            client = new FtpClient(new NetworkCredential(username, password));

            CreateDirectoryTest(new Uri(new Uri(hostAddress), "/TestDirectory1"));
            DeleteDirectoryTest(new Uri(new Uri(hostAddress), "/TestDirectory1"));

            Console.WriteLine();
            Console.WriteLine("Integration test finished.");
            Console.ReadLine();
        }

        private static void CreateDirectoryTest(Uri directory)
        {
            client.CreateDirectory(directory);

            bool exists = client.DirectoryExists(directory);
            Console.WriteLine("CreateDirectory method has " + (exists ? "SUCCEED" : "FAILED"));
        }

        private static void DeleteDirectoryTest(Uri directory)
        {
            bool succeed = false;

            if (client.DirectoryExists(directory))
            {
                client.DeleteDirectory(directory);

                succeed = !client.DirectoryExists(directory);
            }

            Console.WriteLine("CreateDirectory method has " + (succeed ? "SUCCEED" : "FAILED"));
        }
    }
}