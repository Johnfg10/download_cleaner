using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadCleanerCore.Parameters
{
    public class ParameterPhraser
    {
        public static Dictionary<string, string> PhaseParamaters(string strings)
        {
            var stringSplit = strings.Split(' ');
            Dictionary<string, string> keysDictionary = new Dictionary<string, string>();
            foreach (var str in stringSplit)
            {
                var stringSplit1 = str.Split('=');
                if (stringSplit1.Length == 1 && string.IsNullOrWhiteSpace(stringSplit1[0]))
                {
                    keysDictionary.Add(stringSplit1[0], "");
                }
                else if (stringSplit1.Length >= 2)
                {
                    keysDictionary.Add(stringSplit1[0], stringSplit1[1] ?? "");
                }
            }

            return keysDictionary;
        }

        public static Dictionary<string, string> PhaseParamaters(string[] strings)
        {
            var str = "";
            foreach (var s in strings)
            {
                str += " " + s;
            }
            return PhaseParamaters(str);
        }
    }
}
