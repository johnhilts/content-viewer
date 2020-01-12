using System.Threading.Tasks;

namespace dotnet.Libraries.Utilities
{
    public class ExInfoHelper
    {

        public async Task<string> ExtractGeoCoordinates(string fileName)
        {
            return @"N 33 53' 2.2"", E 130 52' 0.7""";
        }

        public async Task<(double Latitude, double Longitude)> 
            ExtractGeoDecimalCoordinates(string fileName)
        {
            return (33 + 53/60 + 2.2/3600, 130 + 52/60 + 0.7/3600);
        }
    }

}

