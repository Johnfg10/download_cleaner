using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Syroot.Windows.IO;

namespace DownloadCleanerCore
{
    class ReplacementHelper
    { 
        public static string ReplaceStrings(string source)
        {
            source = source.Replace("%DOWNLOADS%", KnownFolders.Downloads.Path);
            source = source.Replace("%PERSONAL%", Environment.GetFolderPath(Environment.SpecialFolder.Personal));
            source = source.Replace("%MYDOCS%", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            source = source.Replace("%MYMUSIC%", Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
            source = source.Replace("%MYCOMPUTER%", Environment.GetFolderPath(Environment.SpecialFolder.MyComputer));
            source = source.Replace("%MYPICTURES%", Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
            source = source.Replace("%MYVIDEOS%", Environment.GetFolderPath(Environment.SpecialFolder.MyVideos));
            return source;
        }

        public static string SanatizePathname(string fileName)
        {
            fileName = Path.GetInvalidPathChars()
                .Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));

            fileName = fileName.Replace(" ", "");

            return fileName;
        }
    }
}
