using System;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotnet.Libraries.Utilities
{
    public class ExInfoHelper
    {

        public string ExtractGeoCoordinatesText(string fileName)
        {
            return ExtractData(fileName);
        }

        public (decimal Latitude, decimal Longitude) ExtractGeoCoordinates(string fileName)
        {
            if (!File.Exists(fileName))
                return (0, 0);

            var location = ExtractLocation(fileName);
            return 
                (
                 location.Latitude.Degrees + location.Latitude.Minutes/60 + location.Latitude.Seconds/3600, 
                 location.Longitude.Degrees + location.Longitude.Minutes/60 + location.Longitude.Seconds/3600
                );
        }

        private string ExtractData(string fileName)
        {
            if (fileName.ToLower().EndsWith(".jpg") || fileName.ToLower().EndsWith(".jpeg") || fileName.ToLower().EndsWith("mov"))
            {
                var model = ExtractLocation(fileName);
                return $"{FormatLocation(model.Latitude)}, {FormatLocation(model.Longitude)}";
            }
            else
                return string.Empty;
        }

        private string FormatLocation(GeoCoordinateModel location)
        {
            return location.Seconds == 0
                ? $"{location.Direction} {location.Degrees} {location.Minutes:0.###}'"
                : $"{location.Direction} {location.Degrees} {location.Minutes:0}' {location.Seconds:0.#}\"";
        }

        private GeoCoordinatePairModel ExtractLocation(string fileName)
        {
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                using (var image = Image.FromStream(fs))
                {
                    Func<bool> HasGpsData = () => 
                        Array.IndexOf<int>(image.PropertyIdList, 1) != -1 &&
                        Array.IndexOf<int>(image.PropertyIdList, 2) != -1 &&
                        Array.IndexOf<int>(image.PropertyIdList, 3) != -1 &&
                        Array.IndexOf<int>(image.PropertyIdList, 4) != -1;

                    if (HasGpsData())
                    {
                        var latitude = ExtractGpsLocation(image.GetPropertyItem(2).Value);
                        latitude.Direction = BitConverter.ToChar(image.GetPropertyItem(1).Value, 0).ToString();
                        var longitude = ExtractGpsLocation(image.GetPropertyItem(4).Value);
                        longitude.Direction = BitConverter.ToChar(image.GetPropertyItem(3).Value, 0).ToString();
                        return new GeoCoordinatePairModel 
                        { 
                            Latitude = latitude, 
                            Longitude = longitude,
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error opening {0} image may be corupted, {1}", fileName, ex.Message);
                throw;
            }

            return new GeoCoordinatePairModel();
        }

        private GeoCoordinateModel ExtractGpsLocation(byte[] gpsValue)
        {
            var dN = BitConverter.ToUInt32(gpsValue, 0);
            var dD = BitConverter.ToUInt32(gpsValue, 4);
            var mN = BitConverter.ToUInt32(gpsValue, 8);
            var mD = BitConverter.ToUInt32(gpsValue, 12);
            var sN = BitConverter.ToUInt32(gpsValue, 16);
            var sD = BitConverter.ToUInt32(gpsValue, 20);

            var deg = dD > 0 ? (decimal)dN / dD : dN; 
            var min = mD > 0 ? (decimal)mN / mD : mN;
            var sec = sD > 0 ? (decimal)sN / sD : sN;

            return new GeoCoordinateModel { Degrees = deg, Minutes = min, Seconds = sec, };
        }

    }

}

