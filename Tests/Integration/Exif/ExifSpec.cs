using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using NSpec;
using FluentAssertions;
using dotnet.Libraries.Utilities;

namespace ez7zu6.Integration.Experience
{
    public class ExifSpec : nspec
    {
        void get_exif_info()
        {
            context["exif info from photo"] = () =>
            {
                itAsync["can extract latitude and longitude"] = async () =>
                {
                    var expected = @"N 33 53' 2.2"", E 130 52' 0.7""";
                    var testFile = @"TestImage.JPG";
                    var helper = new ExInfoHelper();
                    var actual = await helper.ExtractGeoCoordinates(testFile);
                    actual.Should().Be(expected);
                };
                itAsync["can extract decimal coordinates"] = async () =>
                {
                    var expected = (33.000611111111111, 130.00019444444445);
                    var testFile = @"TestImage.JPG";
                    var helper = new ExInfoHelper();
                    var actual = await helper.ExtractGeoDecimalCoordinates(testFile);
                    actual.Should().Be(expected);
                };
            };
        }

    }

}

