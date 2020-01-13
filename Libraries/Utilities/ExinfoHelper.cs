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

        public string ExtractGeoCoordinates(string fileName)
        {
            return ExtractData(fileName);
        }

        public (decimal Latitude, decimal Longitude) ExtractGeoDecimalCoordinates(string fileName)
        {
            if (!File.Exists(fileName))
                return (0, 0);

            var location = ExtractLocation(fileName);
            return (location.Latitude.Degrees + location.Latitude.Minutes/60 + location.Latitude.Seconds/3600, location.Longitude.Degrees + location.Longitude.Minutes/60 + location.Longitude.Seconds/3600);
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
            Image image = null;
            try
            {
                try
                {
                    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    image = Image.FromStream(fs);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error opening {0} image may be corupted, {1}", fileName, ex.Message);
                    throw;
                }

                // Check to see if we have gps data
                if (Array.IndexOf<int>(image.PropertyIdList, 1) != -1 &&
                        Array.IndexOf<int>(image.PropertyIdList, 2) != -1 &&
                        Array.IndexOf<int>(image.PropertyIdList, 3) != -1 &&
                        Array.IndexOf<int>(image.PropertyIdList, 4) != -1)
                {

                    var latitude = ExtractGpsLocation(image.GetPropertyItem(2));
                    latitude.Direction = BitConverter.ToChar(image.GetPropertyItem(1).Value, 0).ToString();
                    var longitude = ExtractGpsLocation(image.GetPropertyItem(4));
                    longitude.Direction = BitConverter.ToChar(image.GetPropertyItem(3).Value, 0).ToString();
                    // Console.WriteLine($"gpsLatitudeRef = {gpsLatitudeRef}\r\nlatitude = {latitude}");
                    return new GeoCoordinatePairModel 
                    { 
                        Latitude = latitude, 
                        Longitude = longitude,
                    };
                }
            }
            catch (Exception ex) { Console.WriteLine("Error processing {0} {1}", fileName, ex.Message); }
            finally
            {
                if(image != null) image.Dispose();
            }
            return new GeoCoordinatePairModel();
        }

        private GeoCoordinateModel ExtractGpsLocation(System.Drawing.Imaging.PropertyItem propertyItem)
        {
            uint dN = BitConverter.ToUInt32(propertyItem.Value, 0);
            uint dD = BitConverter.ToUInt32(propertyItem.Value, 4);
            uint mN = BitConverter.ToUInt32(propertyItem.Value, 8);
            uint mD = BitConverter.ToUInt32(propertyItem.Value, 12);
            uint sN = BitConverter.ToUInt32(propertyItem.Value, 16);
            uint sD = BitConverter.ToUInt32(propertyItem.Value, 20);

            decimal deg;
            decimal min; 
            decimal sec;
            // Found some examples where you could get a zero denominator and no one likes to devide by zero
            if (dD > 0) { deg = (decimal)dN / dD; } else { deg = dN; }
            if (mD > 0) { min = (decimal)mN / mD; } else { min = mN; }
            if (sD > 0) { sec = (decimal)sN / sD; } else { sec = sN; }

            return new GeoCoordinateModel { Degrees = deg, Minutes = min, Seconds = sec, };
        }

        private string DecodeRational64u(System.Drawing.Imaging.PropertyItem propertyItem)
        {
            uint dN = BitConverter.ToUInt32(propertyItem.Value, 0);
            uint dD = BitConverter.ToUInt32(propertyItem.Value, 4);
            uint mN = BitConverter.ToUInt32(propertyItem.Value, 8);
            uint mD = BitConverter.ToUInt32(propertyItem.Value, 12);
            uint sN = BitConverter.ToUInt32(propertyItem.Value, 16);
            uint sD = BitConverter.ToUInt32(propertyItem.Value, 20);

            decimal deg;
            decimal min; 
            decimal sec;
            // Found some examples where you could get a zero denominator and no one likes to devide by zero
            if (dD > 0) { deg = (decimal)dN / dD; } else { deg = dN; }
            if (mD > 0) { min = (decimal)mN / mD; } else { min = mN; }
            if (sD > 0) { sec = (decimal)sN / sD; } else { sec = sN; }

            if (sec == 0) return string.Format("{0} {1:0.###}'", deg, min);
            else return string.Format("{0} {1:0}' {2:0.#}\"", deg, min, sec);
        }

    }

}

