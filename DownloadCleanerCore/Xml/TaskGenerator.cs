using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DownloadCleanerCore.Xml.task;
using Task = System.Threading.Tasks.Task;

namespace DownloadCleanerCore.Xml
{
    public class TaskGenerator
    {

        public async Task<Task<List<TaskSet>>> GenerateTaskSets(string xmlContent)
        {
            XmlDocument doc = new XmlDocument {PreserveWhitespace = false};
            var taskList = new List<Task>();
            List<TaskSet> taskSets = new List<TaskSet>();

            await Task.Run(
                ()=>{ doc.LoadXml(xmlContent); }
                );

            var childNodes = doc.ChildNodes;
            if (childNodes.Count == 0)
                return null;

            var tasks = doc.GetElementsByTagName("tasks")[0];

            var tasksChildNodes = tasks.ChildNodes;
            //get each child taskset
            for (int i = tasksChildNodes.Count - 1; i >= 0; i--)
            {
                var childNode = tasksChildNodes[i];
               
                if (childNode.NodeType == XmlNodeType.Element)
                {
                    taskList.Add(Task.Run(() =>
                    {
                        TaskSet taskSet = new TaskSet();
                        if (childNode.Attributes?["name"] != null)
                            taskSet.Name = childNode.Attributes["name"].Value;

                        if (childNode.Attributes?["type"] != null)
                            taskSet.Type = childNode.Attributes["type"].Value;

                        var childNodes1 = childNode.ChildNodes;
                        //get each task
                        for (int o = childNodes1.Count - 1; o >= 0; o--)
                        {
                            var childNode1 = childNodes1[o];
                            if (childNode1.NodeType == XmlNodeType.Element)
                            {
                                if (childNode1.Attributes != null)
                                {
                                    task.Task task = new task.Task();

                                    if (childNode1.Attributes["type"] != null)
                                    {
                                        task.Type = childNode1.Attributes["type"].Value;
                                    }
                                    
                                    if (childNode1.Attributes["method"] != null)
                                    {
                                        task.Method = childNode1.Attributes["method"].Value;
                                    }

                                    var firstNode = childNode1.FirstChild;
                                    if (firstNode?.NodeType == XmlNodeType.Text)
                                    {
                                        task.Value = firstNode.Value;
                                    }

                                    taskSet.Tasks.Add(task);
                                }

                            }
                        }
                        taskSets.Add(taskSet);
                    }));
                }
            }

            var task1 = Task.Run(
                () =>
                {
                    Task.WhenAll(taskList.ToArray()).Wait();

                    return taskSets;
                }
                );
            return task1;
        }
    }
}
