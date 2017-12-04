using System.Collections.Generic;

namespace DownloadCleanerCore.Xml.task
{
    public class TaskSet
    {
        public string Name;

        public string Type;

        public List<Task> Tasks = new List<Task>();
    }
}
