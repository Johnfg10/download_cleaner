using DownloadCleanerCore.taskHandler.tasks;
using System;

namespace DownloadCleanerCore.taskHandler
{
    public class TaskHandlerCore
    {
        public enum TaskType
        {
            File,
            Directory,
            Recycling
        }

        public enum TaskMethod
        {
            Clean,
            Delete
        }

        public void HandleTask(string staskType, string staskMethod, string[] args)
        {
            if (staskType.Equals(String.Empty) || staskMethod.Equals(String.Empty))
                throw new ArgumentNullException("staskType or staskMethod was invalid");

            if (Enum.TryParse(staskType, true, out TaskType taskType) == false)
                throw new ArgumentException("stasktype could not be converted to enum");

            if (Enum.TryParse(staskMethod, true, out TaskMethod taskMethod) == false)
                throw new ArgumentException("stasktype could not be converted to enum");

            HandleTask(taskType, taskMethod, args);
        }

        public void HandleTask(TaskType taskType, TaskMethod taskMethod, string[] args)
        {
            switch (taskType)
            {
                case TaskType.File:
                    if (taskMethod == TaskMethod.Delete)
                    {
                        if (args.Length <= 0)
                            throw new ArgumentException("args must be longer then 0!");
                        var fileTask = new FileTask();
                        Console.WriteLine(fileTask.ClearFile(ReplacementHelper.SanatizePathname(args[0])));
                    }
                    break;

                case TaskType.Directory:
                    if (taskMethod == TaskMethod.Delete)
                    {
                        if (args.Length == 0)
                            throw new ArgumentException("args must be longer then 0!");
                        var dirTask = new DirectoryTask();
                        Console.WriteLine(dirTask.DeleteDirectory(ReplacementHelper.SanatizePathname(args[0])));
                    }
                    else if (taskMethod == TaskMethod.Clean)
                    {
                        if (args.Length == 0)
                            throw new ArgumentException("args must be longer then 0!");

                        var dirTask = new DirectoryTask();
                        dirTask.CleanDirectory(ReplacementHelper.SanatizePathname(args[0])).ForEach((dir)=>{ Console.WriteLine(dir); });
                    }
                    break;

                case TaskType.Recycling:
                    var recyclingTask = new RecyclingTask();
                    recyclingTask.ClearRecycling();
                    break;
            }
        }
    }
}
