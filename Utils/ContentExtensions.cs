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
            return Directory.GetFiles(currentFolder)
                .Where(file => !Path.GetFileName(file).StartsWith(@"."))
                .Select(file => 
                        new ContentModel 
                        {
                            Name = Path.GetFileName(file), 
                            ContentType = ContentType.File, 
                            Created = File.GetCreationTime(file).ToShortDateString(),
                        });
        }

    }
}

