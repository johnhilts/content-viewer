using System.IO;
using System.Threading.Tasks;
using System.Text.Json;

namespace dotnet.Utils
{
    public static class JsonExtensions
    {

        public static async Task<T> ReadJson<T>(this string fileName)
        {
            return JsonSerializer.Deserialize<T>((await File.ReadAllTextAsync(fileName)));
        }

        public static async Task WriteJson<T>(this string fileName, T info)
        {
            await File.WriteAllTextAsync(fileName, JsonSerializer.Serialize(info));
        }

    }
}


