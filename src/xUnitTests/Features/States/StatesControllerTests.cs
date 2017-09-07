using System.Linq;
using King.David.Consulting.Common.Extensions;
using King.David.Consulting.Travel.Web.Features.States;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using Xunit;

namespace xUnitTests.Features.States
{
    public class StatesControllerTests
    {
        [Fact]
        public void GetHasHttpGetAttribute()
        {
            var sut = new StatesController(null);
            var attribute = sut.GetAttributesOn(x => x.Get(null, null))
                    .OfType<HttpGetAttribute>().SingleOrDefault();
            Assert.NotNull(attribute);
            attribute.ShouldBeOfType(typeof(HttpGetAttribute));
            attribute.Template.ShouldBe("states");
        }

        [Fact]
        public void GetStatesByUserHasHttpGetAttribute()
        {
            var sut = new StatesController(null);
            var attribute = sut.GetAttributesOn(x => x.GetStatesByUser(null, null, null))
                    .OfType<HttpGetAttribute>().SingleOrDefault();
            Assert.NotNull(attribute);
            attribute.ShouldBeOfType(typeof(HttpGetAttribute));
            attribute.Template.ShouldBe("user/{user}/visits/states");
        }
    }
}
