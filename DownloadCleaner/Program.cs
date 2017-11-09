using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DownloadCleaner
{
    class Program
    {
        public static readonly string downloadfolder = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads";
        public static readonly string programFolders = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        public static readonly string downloadCleanerFolder = programFolders + Path.DirectorySeparatorChar + "johnfg10";
        public static readonly string downloadCleanerLocation = downloadCleanerFolder + Path.DirectorySeparatorChar + "downloadcleaner.exe";
        public static readonly string registryName = "DownloadCleaner";

        static void Main(string[] args)
        {
            if(args.Length != 0)
            {
                if (args[0] == "--uninstall")
                {
                    File.Delete(downloadCleanerLocation);
                    Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                    key.DeleteValue(registryName, false);
                    return;
                }
            }

            Console.WriteLine("Clearing downloads folder!");
            if (!File.Exists(downloadCleanerLocation))
            {
                Console.WriteLine("Creating statup!");
                Directory.CreateDirectory(downloadCleanerFolder);
                File.Copy(Assembly.GetExecutingAssembly().Location, downloadCleanerLocation);

                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                key.SetValue(registryName, downloadCleanerLocation);
            }
            Empty(new DirectoryInfo(downloadfolder));
            if (args.Contains("--debug"))
            {
                Console.ReadLine();
            }
            Environment.Exit(0);
        }

        public static void Empty(DirectoryInfo directory)
        {
            foreach (FileInfo file in directory.GetFiles()) file.Delete();
            foreach (DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
        }
    }
}
