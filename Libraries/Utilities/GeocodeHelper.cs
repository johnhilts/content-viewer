using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using dotnet.Libraries.Utilities;

namespace dotnet.Libraries.Utilities
{
    public class GeocodeHelper
    {
        public async Task<string> ReverseGeocode(DecimalCoordinatePairModel coordinateModel)
        {
            var helper = new CacheHelper("./");
            var cachedLocationText = await helper.ReadFromCache(coordinateModel);
            if (string.IsNullOrWhiteSpace(cachedLocationText))
            {
               var locationText = await CallReverseGeocodeApi(coordinateModel);
               await helper.SaveToCache(coordinateModel, locationText);
               return locationText;
            }
            
            return cachedLocationText;
        }

        private async Task<string> CallReverseGeocodeApi(DecimalCoordinatePairModel coordinateModel)
        {
            var url = $"https://maps.googleapis.com/maps/api/geocode/json?latlng={coordinateModel.Latitude},{coordinateModel.Longitude}&language=ja&key={apiKey}";
            var client = new HttpClient();
            var response = await client.GetAsync(url);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            dynamic obj = JsonConvert.DeserializeObject(jsonResponse);
            var result = $"{obj.results[0].address_components[2].long_name} {obj.results[0].address_components[3].long_name} {obj.results[0].address_components[5].long_name}";
            return result;
        }

    }
}
