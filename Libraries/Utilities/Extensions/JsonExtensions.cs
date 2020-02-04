using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace dotnet.Libraries.Utilities.Extensions
{
    public static class JsonExtensions
    {

        public static async Task<T> ReadJson<T>(this string fileName)
        {
            return JsonConvert.DeserializeObject<T>((await File.ReadAllTextAsync(fileName)));
        }

        public static async Task WriteJson<T>(this string fileName, T info)
        {
            await File.WriteAllTextAsync(fileName, JsonConvert.SerializeObject(info));
        }

    }
}


