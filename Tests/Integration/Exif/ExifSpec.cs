using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using NSpec;
using FluentAssertions;
using dotnet.Libraries.Utilities;

namespace ez7zu6.Integration.Exif
{
    public class ExifSpec : nspec
    {
        void get_exif_info()
        {
            context["exif info from photo"] = () =>
            {
                var testFile = @"/home/dotnet/wwwroot/content/photos/TestImage.JPG";

                it["can extract latitude and longitude"] = () =>
                {
                    var expected = @"N 33 53' 2.2"", E 130 52' 0.7""";
                    var helper = new ExInfoHelper();
                    var actual = helper.ExtractGeoCoordinatesText(testFile);
                    actual.Should().Be(expected);
                };

                it["can extract decimal coordinates"] = () =>
                {
                    var expected = (33.883938943142361111111111111m, 130.86684746093750000000000000m);
                    var helper = new ExInfoHelper();
                    var actual = helper.ExtractGeoCoordinates(testFile);
                    actual.Should().Be(expected);
                };

            };

            context["photo without exif info"] = () =>
            {
                var testFile = @"/home/dotnet/wwwroot/content/photos/woman.jpg";

                it["returns empty string"] = () =>
                {
                    var expected = string.Empty;
                    var helper = new ExInfoHelper();
                    var actual = helper.ExtractGeoCoordinatesText(testFile);
                    actual.Should().Be(expected);
                };

                it["returns 0,0 coordinates"] = () =>
                {
                    var expected = (0m, 0m);
                    var helper = new ExInfoHelper();
                    var actual = helper.ExtractGeoCoordinates(testFile);
                    actual.Should().Be(expected);
                };

            };
        }

    }

}

