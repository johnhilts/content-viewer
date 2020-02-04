using System;
using System.IO;

namespace dotnet.Libraries.Utilities.Extensions
{
    public static class FileExtensions
    {
        public static bool Exists(this string path)
        {
            return Directory.Exists(path)
                || File.Exists(path);
        }

    }
}

