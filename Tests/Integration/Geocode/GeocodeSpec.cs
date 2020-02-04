using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using NSpec;
using FluentAssertions;
using dotnet.Libraries.Utilities;

namespace ez7zu6.Integration.Geocode
{
    public class GeocodeSpec : nspec
    {
        void get_geocode_info()
        {
            context["geocode"] = () =>
            {
                var testCoordinates = new DecimalCoordinatePairModel 
                {
                    Latitude = 40.714224m, 
                    Longitude = -73.961452m, 
                };

                itAsync["can save to cache"] = async () =>
                {
                    var expected = @"Brooklyn New York";
                    var testRoot = "./";
                    var helper = new CacheHelper(testRoot);
                    helper.ClearCache();
                    await helper.SaveToCache(testCoordinates, expected);
                    var actual = await helper.ReadFromCache(testCoordinates);
                    actual.Should().Be(expected);
                };

                itAsync["can reverse geocode"] = async () =>
                {
                    var expected = @"Brooklyn New York";
                    var helper = new GeocodeHelper();
                    var actual = await helper.ReverseGeocode(testCoordinates);
                    actual.Should().Be(expected);
                };

            };
        }

    }

}


