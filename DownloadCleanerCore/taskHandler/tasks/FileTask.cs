using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadCleanerCore.taskHandler.tasks
{
    public class FileTask
    {
        public enum FileClearingError
        {
            None,
            FileDoesntExist,
            PathIsInvalid,
            UsedByAnotherProcess,
            LackPermission
        }

        public FileClearingError ClearFile(string path)
        {
            if (!File.Exists(path))
                return FileClearingError.FileDoesntExist;

            try
            {
                File.Delete(path);
            }
            catch (ArgumentNullException e)
            {
                return FileClearingError.PathIsInvalid;
            }
            catch (ArgumentException e)
            {
                return FileClearingError.PathIsInvalid;
            }
            catch (PathTooLongException e)
            {
                return FileClearingError.PathIsInvalid;
            }
            catch (IOException e)
            {
                return FileClearingError.UsedByAnotherProcess;
            }
            catch (UnauthorizedAccessException e)
            {
                return FileClearingError.LackPermission;
            }

            return FileClearingError.None;
        }
    }
}
