using DownloadCleaner.config;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DownloadCleaner
{
    class Program
    {
        public static readonly string programFolders = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        public static readonly string downloadCleanerFolder = programFolders + Path.DirectorySeparatorChar + "johnfg10";

        public static readonly string configFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + Path.DirectorySeparatorChar + "johnfg10";
        public static readonly string cleanerConfig = configFolder + Path.DirectorySeparatorChar + "config.json";

        public static readonly string downloadCleanerLocation = downloadCleanerFolder + Path.DirectorySeparatorChar + "downloadcleaner.exe";
        public static readonly string registryName = "DownloadCleaner";
        private Config config = new Config();

        #region helpers

        enum RecycleFlags : int
        {
            SHERB_NOCONFIRMATION = 0x00000001, // Don't ask for confirmation
            SHERB_NOPROGRESSUI = 0x00000001, // Don't show progress
            SHERB_NOSOUND = 0x00000004 // Don't make sound when the action is executed
        }

        [DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
        static extern uint SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, RecycleFlags dwFlags);

        public static string InsertConfigReplacements(string source)
        {
            string download = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads";
            source = source.Replace("$DOWNLOADS$", download);
            source = source.Replace(@"\", @"\\").Replace(@"/", @"\\");
            return source;
        }



        public static string GetEmbeddedResource(string resourceName, Assembly assembly)
        {
            resourceName = FormatResourceName(assembly, resourceName);
            using (Stream resourceStream = assembly.GetManifestResourceStream(resourceName))
            {
                if (resourceStream == null)
                    return null;

                using (StreamReader reader = new StreamReader(resourceStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private static string FormatResourceName(Assembly assembly, string resourceName)
        {
            return assembly.GetName().Name + "." + resourceName.Replace(" ", "_")
                                                               .Replace("\\", ".")
                                                               .Replace("/", ".");
        }

        public static void Empty(DirectoryInfo directory)
        {
            foreach (FileInfo file in directory.GetFiles()) file.Delete();
            foreach (DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
        }

#endregion

        static void Main(string[] args)
        => new Program().Start(args);

        void Start(string[] args)
        {
            if (!Directory.Exists(configFolder))
                Directory.CreateDirectory(configFolder);

            Console.WriteLine("config location: " + cleanerConfig);

            if (!File.Exists(cleanerConfig))
            {
                string config = GetEmbeddedResource("config.json", Assembly.GetExecutingAssembly());

                File.CreateText(cleanerConfig).Close();

                File.WriteAllText(cleanerConfig, config);
            }

            var configString = InsertConfigReplacements(File.ReadAllText(cleanerConfig));
            Console.WriteLine(configString);
            config = JsonConvert.DeserializeObject<Config>(configString);


            if (args.Contains("--help"))
            {
                Console.WriteLine("Currently supported commands are --cleandir");
                Console.WriteLine("--cleandir=* the start repusents the directory you want cleaned");
            }

            if (args.Contains("--cleandir="))
            {
                foreach (var arg in args)
                {
                    if (arg.Contains("--cleandir="))
                    {
                        var argument = arg.Split('=')[1];
                        if (Directory.Exists(argument))
                        {
                            Empty(new DirectoryInfo(argument));
                        }
                    }
                }
            }

            if (args.Contains("--clean"))
            {

            }


            if (args.Length == 0)
            {
                ConfigClean("standard_clean");
                Console.Read();
            }

            Environment.Exit(0);
        }
        //standard is standard_clean
        public void ConfigClean(string configSection)
        {
            var cnfSection = config.Find((ConfigCleaningSection con) => { return con.name == configSection; });

            if (cnfSection.cleanRecycling)
            {
                Console.WriteLine("Cleaning recycling");
                SHEmptyRecycleBin(IntPtr.Zero, null, RecycleFlags.SHERB_NOCONFIRMATION);
            }

            foreach(var dir in cnfSection.dirsToClean)
            {
                var dirInfo = new DirectoryInfo(dir);
                Empty(dirInfo);
            }

        }

    }
}
