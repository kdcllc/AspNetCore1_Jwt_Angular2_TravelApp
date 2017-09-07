using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using King.David.Consulting.Common.Extensions;
using King.David.Consulting.Travel.Web.Features.Citities;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using Xunit;

namespace xUnitTests.Features.Cities
{
    public class CitiesControllerTests
    {
        [Fact]
        public void GetHasHttpGetAttribute()
        {
            var sut = new CitiesController(null);
            var attribute = sut.GetAttributesOn(x => x.Get(null,null))
                    .OfType<HttpGetAttribute>().SingleOrDefault();
            Assert.NotNull(attribute);
            attribute.ShouldBeOfType(typeof(HttpGetAttribute));
            attribute.Template.ShouldBe("cities");
        }

        [Fact]
        public void GetCitiesByStateHasHttpGetAttribute()
        {
            var sut = new CitiesController(null);
            var attribute = sut.GetAttributesOn(x => x.GetCitiesByStateRequired(null,null,null))
                    .OfType<HttpGetAttribute>().SingleOrDefault();
            Assert.NotNull(attribute);
            attribute.ShouldBeOfType(typeof(HttpGetAttribute));
            attribute.Template.ShouldBe("state/{state}/cities");
        }
    }
}
