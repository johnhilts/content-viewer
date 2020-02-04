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
        private readonly string _cacheDirectory = "./cacheDir";
        
        public CacheHelper() : this(string.Empty) { }
        
        public CacheHelper(string cacheRoot)
        {
            _cacheRoot = cacheRoot;
        }

        public async Task ClearCache()
        {
            var folders = await GetExistingFolders();
            folders.ForEach(folder => {
                    var files = Directory.GetFiles(folder);
                    // files.ForEach(file => System.IO.File.Delete(file)); <-- what was wrong with this?!?
                    foreach (var file in files) File.Delete(file);
                    Directory.Delete(folder);
            });
            File.Delete(_cacheDirectory);
        }

        public async Task SaveToCache(DecimalCoordinatePairModel coordinateModel, string locationText)
        {
            var folderName = GetFolderName(coordinateModel.Latitude);
            if (!folderName.Exists()) {
                Directory.CreateDirectory(folderName);
                await UpdateCacheDirectory(folderName);
            }

            var entry = new ReverseGeocodeModel {Latitude = coordinateModel.Latitude, Longitude = coordinateModel.Longitude, LocationText = locationText, };
            var fileName = GetFileName(folderName, coordinateModel.Latitude);
            if (!fileName.Exists()) {
                var entries = new List<ReverseGeocodeModel> { entry };
                await fileName.WriteJson(entries);
            }

            var fileContents = await fileName.ReadJson<List<ReverseGeocodeModel>>();
            if (!fileContents.Exists(fc => fc.Longitude == coordinateModel.Longitude)) {
                fileContents.Add(entry);
                await fileName.WriteJson(fileContents);
            }
        }

        private async Task<List<string>> GetExistingFolders() => _cacheDirectory.Exists() ? await _cacheDirectory.ReadJson<List<string>>() : new List<string>();

        private async Task UpdateCacheDirectory(string folderName)
        {
            var folders = await GetExistingFolders();
            folders.Add(folderName);
            await _cacheDirectory.WriteJson(folders);
        }

        public async Task<string> ReadFromCache(DecimalCoordinatePairModel coordinateModel)
        {

            var folderName = GetFolderName(coordinateModel.Latitude);
            if (!folderName.Exists()) {
                return null;
            }

            var fileName = GetFileName(folderName, coordinateModel.Latitude);
            if (!fileName.Exists()) {
                return null;
            }

            var fileContents = await fileName.ReadJson<List<ReverseGeocodeModel>>();
            if (fileContents.Exists(entry => entry.Longitude == coordinateModel.Longitude)) {
                return fileContents.Single(entry => entry.Longitude == coordinateModel.Longitude).LocationText;
            }

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

