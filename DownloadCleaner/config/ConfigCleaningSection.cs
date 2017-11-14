using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadCleaner.config
{
    public class ConfigCleaningSection
    {
        public string name;

        public bool cleanRecycling;

        public IList<string> dirsToClean = new List<string>();
    }
}
