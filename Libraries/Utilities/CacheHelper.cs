using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using dotnet.Libraries.Utilities.Extensions;

namespace dotnet.Libraries.Utilities
{
    public class CacheHelper
    {
        private readonly string _cacheRoot;
        
        public CacheHelper() : this(string.Empty) { }
        
        public CacheHelper(string cacheRoot)
        {
            _cacheRoot = cacheRoot;
        }

        public void ClearCache()
        {
        }

        public async Task SaveToCache(DecimalCoordinatePairModel model, string locationText)
        {
            var folderName = GetFolderName(model.Latitude);
            if (!folderName.Exists()) {
                Directory.CreateDirectory(folderName);
            }

            var entry = new ReverseGeocodeModel {Latitude = model.Latitude, Longitude = model.Longitude, LocationText = locationText, };
            var fileName = GetFileName(folderName, model.Latitude);
            if (!fileName.Exists()) {
                var entries = new List<ReverseGeocodeModel> { entry };
                await fileName.WriteJson(entries);
            }

            var fileContents = await fileName.ReadJson<List<ReverseGeocodeModel>>();
            if (!fileContents.Exists(fc => fc.Longitude == model.Longitude)) {
                fileContents.Add(entry);
                await fileName.WriteJson(fileContents);
            }
        }

        public async Task<string> ReadFromCache(DecimalCoordinatePairModel model)
        {

            var folderName = GetFolderName(model.Latitude);
            if (!folderName.Exists()) {
                Console.WriteLine("Folder name !exist!");
                return null;
            }

            var fileName = GetFileName(folderName, model.Latitude);
            if (!fileName.Exists()) {
                Console.WriteLine("File name !exist!");
                return null;
            }

            var fileContents = await fileName.ReadJson<List<ReverseGeocodeModel>>();
            if (fileContents.Exists(entry => entry.Longitude == model.Longitude)) {
                return fileContents.Single(entry => entry.Longitude == model.Longitude).LocationText;
            }

                Console.WriteLine("No matching entry!");

            return null;
        }

        private string GetFolderName(decimal latitude)
        {
            var prefix = string.IsNullOrWhiteSpace(_cacheRoot) ? string.Empty : $"{_cacheRoot}";
            return Path.Combine(prefix, Math.Floor(latitude).ToString());
        }

        private string GetFileName(string folderName, decimal latitude)
        {
            var containingFolderName = folderName.Split('/').Last();
            var fileName = latitude.ToString().Replace(containingFolderName, string.Empty).Replace(".", string.Empty);
            return $"{folderName}/{fileName}";
        }

    }
}

