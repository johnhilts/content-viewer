using System.Linq;
using System.Collections.Generic;
using System.IO;
using dotnet.Models;

namespace dotnet.Utils
{
    public static class FileUtility
    {
        public static string MapContentFolder(string physicalRelativeFolder)
        {
            return physicalRelativeFolder.Replace(@"./wwwroot/", string.Empty);
        }

        public static bool Exists(string path)
        {
            return Directory.Exists(path);
        }

        public static IEnumerable<ContentModel> GetFolders(string currentFolder)
        {
            var folders = Directory.GetDirectories(currentFolder);
            return folders.Select(s => new ContentModel {Name = Path.GetFileName(s), ContentType = ContentType.Folder, });
        }

        public static IEnumerable<ContentModel> GetFiles(string currentFolder)
        {
            var files = Directory.GetFiles(currentFolder);
            return files.Select(s => new ContentModel {Name = Path.GetFileName(s), ContentType = ContentType.File, });
        }

    }
}

