using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DownloadCleanerCore.Parameters;
using DownloadCleanerCore.taskHandler;
using DownloadCleanerCore.taskHandler.tasks;
using DownloadCleanerCore.Xml;
using Syroot.Windows.IO;

namespace DownloadCleanerCore
{
    class Program
    {
        private static readonly string AppDataLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Johnfg10", "DownloadCleaner");
        private static readonly string ConfigLocation = Path.Combine(AppDataLocation, "DownloadCleaner.json");
        private static readonly string XmlLocation = Path.Combine(AppDataLocation, "DownloadCleaner.xml");

        static void Main(string[] args) =>new Program().MainP(args).Wait();

        public async Task MainP(String[] args)
        {
            #region field init
            
            if (!Directory.Exists(AppDataLocation))
                Directory.CreateDirectory(AppDataLocation);

            if (!File.Exists(ConfigLocation))
                File.Create(ConfigLocation).Close();

            if (!File.Exists(XmlLocation))
                File.Create(XmlLocation).Close();
            

            if (File.ReadAllText(ConfigLocation) == String.Empty)
            {
                File.WriteAllBytes(ConfigLocation, Properties.Resources.Config);
            }
            if (File.ReadAllText(XmlLocation) == String.Empty)
            {
                File.WriteAllText(XmlLocation, Properties.Resources.Config1);
            }
            
            #endregion

            Console.WriteLine(KnownFolders.Downloads.Path);
            var xmlText = ReplacementHelper.ReplaceStrings(File.ReadAllText(XmlLocation));

            TaskGenerator taskGenerator = new TaskGenerator();
            var tasksets = await await taskGenerator.GenerateTaskSets(xmlText);
            TaskHandlerCore taskHandlerCore = new TaskHandlerCore();

            var paras = ParameterPhraser.PhaseParamaters(args);

            if (paras.ContainsKey("run"))
            {
                var taskName = paras["run"];
                var taskSet = tasksets.Find(set =>
                {
                    if (set.Name.Equals(taskName))
                    {
                        return true;
                    }
                    return false;
                });

                foreach (var taskSetTask in taskSet.Tasks)
                {
                    taskHandlerCore.HandleTask(taskSetTask.Type, taskSetTask.Method, new[] { taskSetTask.Value });
                }
            }
            else
            {
                //default task it is then
                var startupTasks = tasksets.FindAll((set => {
                    if (set.Type.Equals("Startup", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                    return false;
                }));

                foreach (var startupTask in startupTasks)
                {
                    foreach (var startupTaskTask in startupTask.Tasks)
                    {
                        taskHandlerCore.HandleTask(startupTaskTask.Type, startupTaskTask.Method, new[] { startupTaskTask.Value });
                    }
                }
            }
            

        }
    }
}
