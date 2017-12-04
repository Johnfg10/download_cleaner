using System;
using System.Collections.Generic;
using System.IO;

namespace DownloadCleanerCore.taskHandler.tasks
{
    public class DirectoryTask
    {
        public enum DirectoryClearingError
        {
            None,
            DirectoryDoesntExist,
            PathIsInvalid,
            UsedByAnotherProcess,
            LackPermission
        }

        public DirectoryClearingError DeleteDirectory(string path)
        {
            if (!Directory.Exists(path))
                return DirectoryClearingError.DirectoryDoesntExist;

            try
            {
                Directory.Delete(path, true);
            }
            catch (UnauthorizedAccessException e)
            {
                return DirectoryClearingError.LackPermission;
            }
            catch (ArgumentNullException e)
            {
                return DirectoryClearingError.PathIsInvalid;
            }
            catch (ArgumentException e)
            {
                return DirectoryClearingError.PathIsInvalid;
            }
            catch (PathTooLongException e)
            {
                return DirectoryClearingError.PathIsInvalid;
            }
            catch (DirectoryNotFoundException e)
            {
                return DirectoryClearingError.DirectoryDoesntExist;
            }
            catch (IOException e)
            {
                return DirectoryClearingError.UsedByAnotherProcess;
            }

            return DirectoryClearingError.None;
        }

        public List<DirectoryClearingError> CleanDirectory(string path)
        {
            List<DirectoryClearingError> directoryClearingErrors = new List<DirectoryClearingError>();

            if (!Directory.Exists(path))
                directoryClearingErrors.Add(DirectoryClearingError.DirectoryDoesntExist);

            Console.WriteLine("\"" + path + "\"");

            var files = Directory.GetFiles(path);

            foreach (var file in files)
            {
                try
                {
                    File.Delete(file);
                }
                catch (UnauthorizedAccessException e)
                {
                    directoryClearingErrors.Add(DirectoryClearingError.LackPermission);
                }
                catch (ArgumentNullException e)
                {
                    directoryClearingErrors.Add(DirectoryClearingError.PathIsInvalid);
                }
                catch (ArgumentException e)
                {
                    directoryClearingErrors.Add(DirectoryClearingError.PathIsInvalid);
                }
                catch (PathTooLongException e)
                {
                    directoryClearingErrors.Add(DirectoryClearingError.PathIsInvalid);
                }
                catch (DirectoryNotFoundException e)
                {
                    directoryClearingErrors.Add(DirectoryClearingError.DirectoryDoesntExist);
                }
                catch (IOException e)
                {
                    directoryClearingErrors.Add(DirectoryClearingError.UsedByAnotherProcess);
                }
            }

            var dirs = Directory.GetDirectories(path);
            
            foreach (var dir in dirs)
            {
                try
                {
                    Directory.Delete(dir);
                }
                catch (UnauthorizedAccessException e)
                {
                    directoryClearingErrors.Add(DirectoryClearingError.LackPermission);
                }
                catch (ArgumentNullException e)
                {
                    directoryClearingErrors.Add(DirectoryClearingError.PathIsInvalid);
                }
                catch (ArgumentException e)
                {
                    directoryClearingErrors.Add(DirectoryClearingError.PathIsInvalid);
                }
                catch (PathTooLongException e)
                {
                    directoryClearingErrors.Add(DirectoryClearingError.PathIsInvalid);
                }
                catch (DirectoryNotFoundException e)
                {
                    directoryClearingErrors.Add(DirectoryClearingError.DirectoryDoesntExist);
                }
                catch (IOException e)
                {
                    directoryClearingErrors.Add(DirectoryClearingError.UsedByAnotherProcess);
                }
            }

            return directoryClearingErrors;
        }

    }
}
