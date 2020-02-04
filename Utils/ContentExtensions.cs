using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using dotnet.Models;
using dotnet.Libraries.Utilities;

namespace dotnet.Utils
{
    public static class ContentExtensions
    {
        public static string MapContentFolder(this string physicalRelativeFolder)
        {
            return physicalRelativeFolder.Replace(@"./wwwroot/content", string.Empty);
        }

        public static IEnumerable<ContentModel> GetFolders(this string currentFolder)
        {
            var folders = Directory.GetDirectories(currentFolder);
            return folders.Select(folder => new ContentModel {Name = Path.GetFileName(folder), ContentType = ContentType.Folder, });
        }

        public static async Task<IEnumerable<ContentModel>> GetFiles(this string currentFolder)
        {
            return 
                (await Task.WhenAll(
                    Directory.GetFiles(currentFolder)
                    .Where(file => !Path.GetFileName(file).StartsWith(@"."))
                    .Select(async (file) =>  
                    {
                        var geoCoordinateText = string.Empty;
                        try
                        {
                            var coordinateModel = (new ExInfoHelper()).ExtractGeoCoordinates(file);
                            try
                            {
                                geoCoordinateText = (await (new GeocodeHelper()).ReverseGeocode(coordinateModel));
                            }
                            catch (Exception exception)
                            {
                                Console.WriteLine($"reverse geo-coding error: {exception.ToString()}");
                            }
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine($"file io error: {exception.ToString()}");
                        }
                        return new ContentModel 
                            {
                                Name = Path.GetFileName(file), 
                                ContentType = ContentType.File, 
                                Created = File.GetCreationTime(file).ToShortDateString(),
                                GeoCoordinateText = geoCoordinateText,
                            };
                    })
                ));
        }

    }
}

