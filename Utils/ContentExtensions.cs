using System.Linq;
using System.Collections.Generic;
using System.IO;
using dotnet.Models;

namespace dotnet.Utils
{
    public static class ContentExtensions
    {
        public static string MapContentFolder(this string physicalRelativeFolder)
        {
            return physicalRelativeFolder.Replace(@"./wwwroot/content", string.Empty);
        }

        public static bool Exists(this string path)
        {
            return Directory.Exists(path)
                || File.Exists(path);
        }

        public static IEnumerable<ContentModel> GetFolders(this string currentFolder)
        {
            var folders = Directory.GetDirectories(currentFolder);
            return folders.Select(folder => new ContentModel {Name = Path.GetFileName(folder), ContentType = ContentType.Folder, });
        }

        public static IEnumerable<ContentModel> GetFiles(this string currentFolder)
        {
            var fileNames = Directory.GetFiles(currentFolder).Select(file => Path.GetFileName(file));
            return fileNames.Where(file => !file.StartsWith(@".")).Select(fileName => new ContentModel {Name = fileName, ContentType = ContentType.File, });
        }

    }
}

