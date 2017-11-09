using System;
using System.IO;
using System.Reflection;

namespace Download_Cleaner
{
    class Program
    {
        public static readonly string downloadfolder = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads";
        public static readonly DirectoryInfo downloadFolder = Directory.CreateDirectory(downloadfolder);
        public static readonly string startupfolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
        public static readonly string startupLocation = startupfolder + Path.DirectorySeparatorChar + "download_cleaner.exe";

        static void Main(string[] args)
        {
            Console.WriteLine(startupLocation);
            Console.WriteLine("Clearing downloads folder!");
            if (!File.Exists(startupLocation))
            {
                Console.WriteLine("Creating statup!");
                File.Copy(Assembly.GetExecutingAssembly().Location, startupLocation);
            }
            Empty(downloadFolder);
        }

        public static void Empty(DirectoryInfo directory)
        {
            foreach (FileInfo file in directory.GetFiles()) file.Delete();
            foreach (DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
        }
    }
}
