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

        public async Task<string> ExtractGeoCoordinates(string fileName)
        {
            return ExtractData(fileName);
        }

        public async Task<(double Latitude, double Longitude)> 
            ExtractGeoDecimalCoordinates(string fileName)
        {
            if (!File.Exists(fileName))
                return (0, 0);

            return (33 + 53/60 + 2.2/3600, 130 + 52/60 + 0.7/3600);
        }

        private string ExtractData(string fileName)
        {
            if (fileName.ToLower().EndsWith(".jpg") || fileName.ToLower().EndsWith(".jpeg") || fileName.ToLower().EndsWith("mov"))
            {
                return ExtractLocation(fileName);
            }
            else
                return string.Empty;

        }

        private string ExtractLocation(string fileName)
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

                    string gpsLatitudeRef = BitConverter.ToChar(image.GetPropertyItem(1).Value, 0).ToString();
                    string latitude = DecodeRational64u(image.GetPropertyItem(2));
                    string gpsLongitudeRef = BitConverter.ToChar(image.GetPropertyItem(3).Value, 0).ToString();
                    string longitude = DecodeRational64u(image.GetPropertyItem(4));
                    Console.WriteLine($"gpsLatitudeRef = {gpsLatitudeRef}\r\nlatitude = {latitude}");
                    return $"{gpsLatitudeRef} {latitude}, {gpsLongitudeRef} {longitude}";
                }
            }
            catch (Exception ex) { Console.WriteLine("Error processing {0} {1}", fileName, ex.Message); }
            finally
            {
                if(image != null) image.Dispose();
            }
            return string.Empty;
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

